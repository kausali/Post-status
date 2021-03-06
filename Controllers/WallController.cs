using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wall.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace wall.Controllers
{
    public class WallController : Controller
    {
        private UserContext _context;
 
        public WallController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("the_wall")]
        public IActionResult Whatever()
        {
            var mySession = HttpContext.Session.GetInt32("currUserID");
            if(mySession == null){
                TempData["wallError"] = "Sorry you are currently not logged in. Please login first!";
                return RedirectToAction("Index", "Home");
            }
            var CurrUser = _context.users.Where(u => u.userID == mySession).SingleOrDefault();
            ViewBag.CurrUser = CurrUser;
            Console.Write("********************");
            List<Message> allMessages = _context.messages.OrderByDescending(z =>z.messageID)
                                                        .Include(a =>a.User)
                                                        .Include(s =>s.Comments)
                                                        .ToList();

            List<Comment> allComments = _context.comments.Include(a =>a.User)
                                                        .Include(s =>s.Message)
                                                        .ToList();
            ViewBag.allComments = allComments;    
            ViewBag.allMessages = allMessages;
            return View("Wall");
        }

        [HttpPost]
        [Route("post")]
        public IActionResult Post(Message newMessage)
        {
            var mySession = HttpContext.Session.GetInt32("currUserID");
            if(mySession == null){
                TempData["wallError"] = "Sorry you are currently not logged in. Please login first!";
                return RedirectToAction("Index", "Home");
            }

            if(ModelState.IsValid){
                int myUserID = (int)HttpContext.Session.GetInt32("currUserID");
                newMessage.UserID = myUserID;
                newMessage.created_at = DateTime.Now;
                newMessage.updated_at = DateTime.Now;
                _context.Add(newMessage);
                _context.SaveChanges();
                return RedirectToAction("Whatever");
            }
            if(!ModelState.IsValid){
                 TempData["messageError"] = "Message should be atleast 3 characters long!";
                return RedirectToAction ("Whatever");
            }
            return View("Wall");
        }

        [HttpPost]
        [Route("comment/{msg_id}")]
        public IActionResult Comment(string comment, int msg_id)
        {
            var mySession = HttpContext.Session.GetInt32("currUserID");
            if(mySession == null){
                TempData["commentError"] = "Sorry you are currently not logged in. Please login first!";
                return RedirectToAction("Index", "Home");
            }
            if(comment == null){
                TempData["commentError"] = "Comment should be atleast 3 characters long.";
                return RedirectToAction("Whatever");
            }
            if(ModelState.IsValid){
                int myUserID = (int)HttpContext.Session.GetInt32("currUserID");
                var commentList = _context.comments
                                        .Include(z=>z.User)
                                        .Include(x=>x.Message)
                                        .ToList();
                Comment newComment = new Comment{

                    comment = comment,
                    UserID = myUserID,
                    MessageID = msg_id,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                _context.Add(newComment);
                _context.SaveChanges();
                return RedirectToAction("Whatever");   
            }
            if(!ModelState.IsValid){
                 TempData["commentError"] = "Comment should be atleast 2 characters long!";
                return RedirectToAction ("Whatever");
            }
            return View("Wall");
        }

        [HttpGet]
        [Route("User/Profile/{user_id}/{f_name}-{l_name}")]
        public IActionResult Profile(int user_id, string f_name, string l_name)
        {
            var mySession = HttpContext.Session.GetInt32("currUserID");
            if(mySession == null){
                TempData["wallError"] = "Sorry you are currently not logged in. Please login first!";
                return RedirectToAction("Index", "Home");
            }
            var CurrUser = _context.users.Where(u => u.userID == mySession).SingleOrDefault();
            ViewBag.CurrUser = CurrUser;
            User myUser = _context.users.Where(u=>u.userID == user_id).SingleOrDefault();
            ViewBag.myUser = myUser;
            List<Message> userActivity = _context.messages.OrderByDescending(z=>z.messageID)
                                        .Include(z=> z.User)
                                        .Include(u=>u.Comments)
                                        .ToList();
            List<Comment> myComments = _context.comments.Include(a =>a.User)
                                                        .Include(s =>s.Message)
                                                        .ToList();
            ViewBag.userComments = myComments;
            ViewBag.userActivity = userActivity;
            return View("Profile");
        }


        [HttpGet]
        [Route("MyPage/MyProfile/{f_name}-{l_name}")]
        public IActionResult myPage(string f_name, string l_name)
        {
            var mySession = HttpContext.Session.GetInt32("currUserID");
            if(mySession == null){
                TempData["wallError"] = "Sorry you are currently not logged in. Please login first!";
                return RedirectToAction("Index", "Home");
            }
            User myAccount = _context.users.Where(u=>u.userID == mySession).SingleOrDefault();
            ViewBag.myAccount = myAccount;
            List<Message> myActivities = _context.messages.OrderByDescending(z =>z.messageID)
                                        .Include(z=> z.User)
                                        .Include(u=>u.Comments)
                                        .ToList();
            List<Comment> myComments = _context.comments.Include(a =>a.User)
                                                        .Include(s =>s.Message)
                                                        .ToList();
            // Image aimage = _context.images.Where(u=>u.UserID == mySession).SingleOrDefault();
            // ViewBag.aimage = aimage;
            ViewBag.userComments = myComments;
            ViewBag.myActivies = myActivities;  
            ViewBag.CurrUser = mySession;  
                 
            return View("MyProfile");
        }
        [HttpPost]
        [Route("uploadphoto")]
        public IActionResult Uploadphoto(byte image){
            var mySession = HttpContext.Session.GetInt32("currUserID");
            if(mySession == null){
                TempData["wallError"] = "Sorry you are currently not logged in. Please login first!";
                return RedirectToAction("Index", "Home");
            }
            int myUserID = (int)mySession;
            // Image aimage = _context.images.Where(u=>u.UserID == myUserID).SingleOrDefault();
            // if (aimage == null){
            //     Image newImage = new Image{

            //         image = image,
            //         UserID = myUserID,
            //         created_at = DateTime.Now,
            //         updated_at = DateTime.Now
            //     };
                
            //     _context.Add(newImage);
            //     _context.SaveChanges();
            //     return View("MyProfile");
            // }
            return View("MyProfile");
        }
        [HttpPost]
        [Route("delete/{msgID}")]
        public IActionResult DeleteMessage(int msgID){
            List<Comment> userComent =_context.comments.Include(a =>a.User)
                                                        .Include(s =>s.Message)
                                                        .ToList();
            foreach(var comment in userComent) 
            {
                if(comment.MessageID == msgID){
                    _context.comments.Remove(comment);
                    _context.SaveChanges();
                }
            }
            Message RetrievedMessage = _context.messages.SingleOrDefault(m=>m.messageID == msgID);
            _context.messages.Remove(RetrievedMessage);
            _context.SaveChanges();
            return RedirectToAction("Whatever");
        }
        [HttpPost]
        [Route("delete/comment/{cmtID}")]
        public IActionResult DeleteComment(int cmtID){
            List<Comment> userComent =_context.comments.Include(a =>a.User)
                                                        .Include(s =>s.Message)
                                                        .ToList();
            foreach(var comment in userComent) 
            {
                if(comment.commentID == cmtID){
                    _context.comments.Remove(comment);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Whatever");
        }
    
    }
}
