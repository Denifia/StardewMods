using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Common.Models;
using Denifia.Stardew.SendLetters.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Denifia.Stardew.SendLetters.Domain
{
    public class LocalAndRemoteRepository 
    {
        internal IRestService _restService;

        //public LocalAndRemoteRepository(IConfigurationService configService, 
        //    IRestService restService)
        //    : base(configService)
        //{
        //    _restService = restService;
        //}

        //public override IQueryable<Message> FindMessages(string playerId, Expression<Func<Message, bool>> predicate)
        //{
        //    // Get online messages
        //    var urlSegments = new Dictionary<string, string>();
        //    urlSegments.Add("playerId", playerId);
        //    _restService.GetRequest<List<Message>>("Messages/ToPlayer/{playerId}", urlSegments, RemoteMessagesRetreived);

        //    return base.FindMessages(playerId, predicate);
        //}

        //private void RemoteMessagesRetreived(List<Message> remoteMessages)
        //{
        //    // Add remote messages to local db
        //    if (remoteMessages == null) return;
        //    foreach (var message in remoteMessages)
        //    {
        //        base.CreateMessage(message);
        //    }
        //}

        //public override void CreateMessage(Message message)
        //{
        //    base.CreateMessage(message);
            
        //    if (!_database.Players.Any(x => x.Id == message.ToPlayerId))
        //    {
        //        // Message destined for remote player
        //        var messageCreateModel = new MessageCreateModel
        //        {
        //            ToPlayerId = message.ToPlayerId,
        //            FromPlayerId = message.FromPlayerId,
        //            Text = message.Text
        //        };

        //        var urlSegments = new Dictionary<string, string>();
        //        _restService.PostRequest("Messages", urlSegments, messageCreateModel, ModEvents.RaiseMessageSentEvent);
        //    }
        //}

        //public override void Delete(Message message)
        //{
        //    base.Delete(message);
        //    // online delete message

        //    var urlSegments = new Dictionary<string, string>();
        //    urlSegments.Add("messageId", message.Id);
        //    _restService.DeleteRequest("Messages/{messageId}", urlSegments);
        //}
    }
}

