using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prsCapstone.Controllers;
using prsCapstone.Model;

namespace TestPrsCapstone
{
    public class UnitTest1
    {
        //public async Task<ActionResult<IEnumerable<Request>>> GetReviews(int id)
        //{
        //    return await (from r in _context.Requests
        //                  where r.Id != id
        //                  select r).ToListAsync();
        //}
        //public async Task<ActionResult<User>> Login(string username, string password)
        //{
        //    var user = await (from u in _context.Users
        //                      where u.Username == username && u.Password == password
        //                      select u).SingleOrDefaultAsync();

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return user;
        //}
        UsersController usersController = default!;
        RequestsController rcontroller = default!;
        
        public UnitTest1()
        {
            var x = usersController.User;
        }
        [Theory]
        [InlineData("test","test")]
        public void TestLogin(string username, string password)
        {
            var test = usersController.Login(username, password);
            Assert.NotNull(test);

        }
        //[Theory]
        //[InlineData(1)]
        //public void TestGetReviews(int id)
        //{
        //    var test = (from r in
        //                  where r.Id != id
        //                  select r).ToListAsync();
        //   rcontroller.GetReviews(id);
        //}
    }
}