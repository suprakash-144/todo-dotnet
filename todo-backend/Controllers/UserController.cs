using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_backend.Dto;
using todo_backend.Services.Interface;
using todo_backend.Validator;

namespace todo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UserController> _logger = logger;

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserResponse>> SignupUser(CreateUserRequest dto)
        {
            CoustomValidation validator = new();
            ValidationResult  result = validator.Validate(dto);
            if (result.IsValid)
            {

                try
                {
                    var user = await _userService.CreateUser(dto);
                    var res = new CreateUserResponse { Email = user.Email, Name = user.Name, Password = user.Password };
                    return CreatedAtAction(nameof(SignupUser), res);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                }
            else
            {
                return BadRequest( result.Errors.FirstOrDefault()?.ErrorMessage );
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginUserResponse>> LoginUser(LoginUserRequest dto)
        {
            var result = await _userService.LoginUser(dto);
            if (result is null)
            {
                return BadRequest("Invalid Credentials");

            }
            //HttpContext.Response.Cookies.Append("token", result.Token ,new CookieOptions()
            //{
            //    SameSite = SameSiteMode.None,
            //    //IsEssential = true,
            //    HttpOnly = true,
            //    Expires = DateTime.UtcNow.AddDays(3),
            //    Secure = true
            //});
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult >Refresh()
        {
            bool exist = HttpContext.Request.Cookies.TryGetValue("token", out var tokens);
            if (exist && !String.IsNullOrEmpty(tokens))
            {

                var Result = await _userService.HandleRefresh(tokens.ToString());
                if (Result is null)
                {
                    return StatusCode(500,"Internal Server Error");
                }
                return Ok(new{ accessToken = Result });
                //return Ok(new{token= tokens });
            }
            return StatusCode(500,"Token not Included");
        }
        [HttpGet]
        public IActionResult Logout()
        {

            bool exist = HttpContext.Request.Cookies.TryGetValue("token", out var tokens);
            if (exist)
            {
                HttpContext.Response.Cookies.Delete("token", new CookieOptions
                {
                    SameSite = SameSiteMode.None,
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(3),
                    Secure = true
                });
                return Ok();
            }

            return StatusCode(500,"Token does not exist");


        }
    }
}
