using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Services
{
    public class UiMessagingPipeline
    {
        private readonly Dictionary<string, List<UiMessage>> _messagesPerUserMemory = new Dictionary<string, List<UiMessage>>();

        public void AddUiMessageForUser(string userId, params UiMessage[] messagesToAdd)
        {
            if (_messagesPerUserMemory.ContainsKey(userId))
            {
                var messageListForUser = _messagesPerUserMemory[userId];
                messageListForUser ??= new List<UiMessage>();
                
                messageListForUser.AddRange(messagesToAdd);

                _messagesPerUserMemory[userId] = messageListForUser;
            }

            else
            {
                _messagesPerUserMemory.Add(userId, messagesToAdd.ToList());
            }
        }

        public List<UiMessage> GetUiMessagesForUser(string userId)
        {
            if(userId == null)
                throw new ArgumentNullException(nameof(userId));
            
            var hasMessagesForUser = _messagesPerUserMemory.TryGetValue(userId, out var messages);

            _messagesPerUserMemory.Remove(userId);
            
            return hasMessagesForUser ? messages : new List<UiMessage>();
        }
    }

    public class UiMessage
    {
        public UiMessage(string content, UiMessageType type = UiMessageType.Error)
        {
            Type = type;
            Content = content;
        }

        public string Content { get; set; }
        public UiMessageType Type { get; set; }
    }

    public enum UiMessageType
    {
        Info,
        Warning,
        Error
    }
}