using System;
using System.Globalization;
using System.Net.Http;
using System.Reflection.Metadata;
using AutoMapper;
using DatingApp.Api.Data.Persistence;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;
using DatingApp.Api.Helpers;
using DatingApp.Api.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    public class MessagesController : DatingAppController
    {
        private readonly IMessageRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IMapper mapper,
            IMessageRepository repository,
            IUserRepository userRepository,
            ILogger<MessagesController> logger
        )
        {
            _mapper = mapper;
            _repository = repository;
            _userRepository = userRepository;
            _logger = logger;
        }

        [ProducesResponseType(typeof(IEnumerable<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("thread/{recipientUserId:Guid}")]
        public async Task<IActionResult> GetMessagesThread(Guid recipientUserId, CancellationToken cancellationToken)
        {
            var messagesThread = await _repository.GetMessageThreadAsync(User.GetUserId().Value, recipientUserId, cancellationToken);

            return Ok(messagesThread);
        }

        [ProducesResponseType(typeof(PagedList<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery]MessageParams messageParams, CancellationToken cancellationToken)
        {
            messageParams.UserId = User.GetUserId().Value;

            var messagesPagedList = await _repository.GetPaginatedAsync(messageParams, cancellationToken);

            HttpContext.Response.AddPaginationHeader(
                messagesPagedList.ItemCount,
                messagesPagedList.PageCount,
                messagesPagedList.PageSize,
                messagesPagedList.PageNumber
            );

            return Ok(messagesPagedList);
        }

        [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageInputModel inputModel, CancellationToken cancellationToken)
        {
            var currentUserId = User.GetUserId().Value;
            if (inputModel.RecipientId == currentUserId)
                return BadRequest("Invalid operation.");

            var recipientUser = await _userRepository.GetByIdAsync(inputModel.RecipientId);
            if (recipientUser is null)
                return NotFound();

            var senderUser = await _userRepository.GetByIdAsync(currentUserId);

            var message = new Message
            {
                Sender = senderUser,
                SenderUserName = senderUser.KnownAs,
                Recipient = recipientUser,
                RecipientUserName = recipientUser.KnownAs,
                Content = inputModel.Content,
            };

            _repository.Add(message);

            if (await _repository.SaveAllAsync(cancellationToken))
                return StatusCode(StatusCodes.Status201Created, _mapper.Map<MessageDto>(message));

            _logger.LogError("Unable to save message");
            return Problem("Unable to save message");
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:Guid}/mark-read")]
        public async Task<IActionResult> MarkRead(Guid id, CancellationToken cancellationToken)
        {
            var message = await _repository.GetByIdAsync(id, cancellationToken);
            if (message is null)
                return NotFound();

            if (message.DateRead.HasValue)
                return NoContent();

            message.DateRead = DateTime.UtcNow;

            _repository.Update(message);
            await _repository.SaveAllAsync(cancellationToken);

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteMessage(Guid id, [FromQuery] bool deletedByRecipient, CancellationToken cancellationToken)
        {
            var message = await _repository.GetByIdAsync(id, cancellationToken);
            if (message is null)
                return NotFound();

            var currentUserId = User.GetUserId();

            if (deletedByRecipient)
            {
                if (message.RecipientId != currentUserId)
                    return Unauthorized();

                message.DeletedByRecipient = true;
            }
            else
            {
                if (message.SenderId != currentUserId)
                    return Unauthorized();

                message.DeletedBySender = true;
            }
            
            _repository.Update(message);

            await _repository.SaveAllAsync(cancellationToken);

            return NoContent();
        }
    }
}