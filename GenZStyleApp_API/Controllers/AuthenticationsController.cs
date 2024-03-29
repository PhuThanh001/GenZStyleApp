using BMOS.BAL.DTOs.Authentications;
using BMOS.BAL.DTOs.JWT;
using FluentValidation;
using FluentValidation.Results;
using GenZStyleAPP.BAL.DTOs.Authencications;
using GenZStyleAPP.BAL.DTOs.Authencications.Response;
using GenZStyleAPP.BAL.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Exceptions;
using ProjectParticipantManagement.BAL.Repositories.Interfaces;
using ProjectParticipantManagement.WebAPI.Helpers;
using System.Net.Http.Headers;

namespace ProjectParticipantManagement.WebAPI.Controllers
{
    public class AuthenticationsController : ODataController
    {
        private IAuthenticationRepository _authenticationRepository;
        private IValidator<GetLoginRequest> _authenticationValidator;
        private IValidator<PostRecreateTokenRequest> _postRecreateTokenValidator;
        private IOptions<JwtAuth> _jwtAuthOptions;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthenticationsController> _logger;
        public AuthenticationsController(IAuthenticationRepository authenticationRepository, 
                                        IValidator<GetLoginRequest> authenticationValidator,
                                        IOptions<JwtAuth> jwtAuthOptions,
                                        IValidator<PostRecreateTokenRequest> postRecreateTokenValidator,
                                        IConfiguration config)
        {
            this._authenticationRepository = authenticationRepository;
            this._authenticationValidator = authenticationValidator;
            this._jwtAuthOptions = jwtAuthOptions;
            _postRecreateTokenValidator = postRecreateTokenValidator;
            _config = config;   
        }

        [EnableQuery]
        [HttpPost("odata/authentications/login")]
        public async Task<IActionResult> Login([FromBody] GetLoginRequest account)
        {   
            try 
            {
             ValidationResult validationResult = await this._authenticationValidator.ValidateAsync(account);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }
            var result = await this._authenticationRepository.LoginAsync(account,_jwtAuthOptions.Value );
            return Ok(result); 
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }
        /*[EnableQuery]
        [HttpPost("odata/authentications/LoginByEmail")]
        public async Task<IActionResult> LoginByEmail([FromBody] GetLoginEmailRequest account)
        {
            try
            {
                ValidationResult validationResult = await this._authenticationValidator.ValidateAsync(account);
                if (!validationResult.IsValid)
                {
                    string error = ErrorHelper.GetErrorsString(validationResult);
                    throw new BadRequestException(error);
                }
                var result = await this._authenticationRepository.LoginAsync(account, _jwtAuthOptions.Value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }*/

        #region Recreate token
        [EnableQuery]
        [HttpPost("odata/authentications/recreate-token")]
        public async Task<IActionResult> RecreateToken([FromBody] PostRecreateTokenRequest request)
        {
            try {
                var validationResult = await _postRecreateTokenValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                string error = ErrorHelper.GetErrorsString(validationResult);
                throw new BadRequestException(error);
            }

            var result = await _authenticationRepository.ReCreateTokenAsync(request, _jwtAuthOptions.Value);
            return Ok(result); 
            
            }
            catch (Exception e) 
            {
               return BadRequest(e);
            }
            
        }
        #endregion
        [HttpPost]
        [Route("google-sign-in")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginRequest request)
        {
            CommonResponse commonResponse = new CommonResponse();
            string internalServerErrorMsg = _config[
                "ResponseMessages:AuthenticationMsg:InternalServerErrorMsg"
            ];
            try
            {
                var clientId = _config["Google:ClientId"];
                var clientSecret = _config["Google:ClientSecret"];
                var authenLink = _config["Google:authenLink"];
                var userInfoLink = _config["Google:userInfo"];
                using (var httpClient = new HttpClient())
                {
                    // Set the authorization header with the access token
                    var client = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        request.GoogleToken
                    );
                    var response = await httpClient.GetAsync(userInfoLink);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var userInfo = JsonConvert.DeserializeObject<GoogleUserInfoResponse>(
                            content
                        );
                        if (userInfo != null && userInfo.Email != null && userInfo.Name != null)
                        {
                            commonResponse = await _authenticationRepository.AuthenticateByGoogleAsync(userInfo, _jwtAuthOptions.Value);
                            switch (commonResponse.Status)
                            {
                                case 200:
                                    return Ok(commonResponse);
                                case 400:
                                    return BadRequest(commonResponse);
                                case 401:
                                    return Unauthorized(commonResponse);
                                case 403:
                                    return StatusCode(403, commonResponse);
                                default:
                                    return StatusCode(500, commonResponse);
                            }
                        }
                        else
                            throw new Exception("Error when parser information");
                    }
                    else
                    {
                        _logger.LogError(
                            "An error occurred: {ErrorMessage}",
                            "Given token google to retrive information failed"
                        );
                        commonResponse.Status = 400;
                        commonResponse.Message = "Given token google to retrive information failed";
                        return StatusCode(400, commonResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                commonResponse.Status = 500;
                commonResponse.Message = internalServerErrorMsg;
            }
            return StatusCode(500, commonResponse);
        }
    }
}
