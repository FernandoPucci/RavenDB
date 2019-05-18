using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabenDb.Api.Infrastructure;
using RabenDb.Api.Model;
using Raven.Client.Documents;

namespace RabenDb.Api.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        [HttpGet]
        public IList Get () {

            IList users = new List<User> ();
            using (var session = DatabaseSessionFactory.OpenSession ()) {

                users = (from user in session.Query<User> () select user).ToList ();
            }

            return users;
        }

        [HttpGet ("{id}")]
        public User GetById (Guid id) {
            using (var session = DatabaseSessionFactory.OpenSession ()) {

                var x = session.Load<User> (id.ToString ());
                return x;

            }
        }

        [HttpPost]
        public ActionResult Post ([FromBody] User user) {

            if (user == null) {
                return NotFound ("Sorry !!");
            }

            if (user != null) {
                user.Id = Guid.NewGuid ().ToString ();
            }

            using (var session = DatabaseSessionFactory.OpenSession ()) {
                session.Store (user);
                session.SaveChanges ();

                return Created (user.Id.ToString (), user);
            }

        }

        [HttpPut ("{id}")]
        public User Put (Guid id, [FromBody] User user) {

            using (var session = DatabaseSessionFactory.OpenSession ()) {

                user.Id = id.ToString ();
                session.Store (user);
                session.SaveChanges ();

                return session.Load<User> (id.ToString ());

            }

        }

        [HttpDelete ("{id}")]
        public ActionResult Delete (Guid id) {

            using (var session = DatabaseSessionFactory.OpenSession ()) {

                var user = session.Load<User> (id.ToString ());

                if (user != null) {
                    
                    session.Delete (user);
                    session.SaveChanges ();
                    return NoContent ();

                } else {

                    return NotFound ();

                }

            }
        }
    }
}