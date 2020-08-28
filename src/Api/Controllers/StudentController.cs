using System;
using System.Collections.Generic;
using System.Linq;
using Logic.AppServices;
using Logic.Dtos;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly Messages _messages;

        public StudentController(UnitOfWork unitOfWork, Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number) //query
        {
            var result = _messages.Dispatch(new GetListQuery(enrolled, number));
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Register([FromBody] NewStudentDto dto) //command
        {
            var result = _messages.Dispatch(new RegisterCommand(
                dto.Name, dto.Email, dto.Course1, dto.Course1Grade, dto.Course2, dto.Course2Grade
                ));

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpDelete("{id}")]
        public IActionResult Unregister(long id)  //command
        {
            var result = _messages.Dispatch(new UnregisterCommand(id));
            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] StudentEnrollmentDto dto) //command
        {
            var command = new EnrollCommand(id, dto.Course, dto.Grade);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)  //command
        {
            var command = new TransferCommand(id, enrollmentNumber, dto.Course, dto.Grade);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/disenroll")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)  //command
        {
            var command = new DisenrollCommand(id, enrollmentNumber, dto.Comment);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)  //command
        {
            var command = new EditPersonalInfoCommand(dto.Email, dto.Name, id);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }
    }
}
