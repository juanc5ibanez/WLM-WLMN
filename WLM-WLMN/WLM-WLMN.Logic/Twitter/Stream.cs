using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using WLM_WLMN.Common.DTOs;
using WLM_WLMN.Common.Interfaces;

namespace WLM_WLMN.Logic.Twitter
{
    public class Stream : ITwitterLogic
    {
        private ITermLogic _termLogic;
        private IUserUpdateStorage _userUpdateStorage;

        public Stream(ITermLogic termLogic, IUserUpdateStorage userUpdateStorage)
        {
            _userUpdateStorage = userUpdateStorage;
            _termLogic = termLogic;
            Auth.SetUserCredentials("ZaBmj9kwJT3Nmo0a50gD4tlsD", "6cS4VvMNWrFT44uGzk4tqvy0DXCGJHyNuKCHDWPH3fIJd0zn6d", "155782351-x2qFIt02BWJUzD0PExndviVgcxdKHEMLLJ7aK8X9", "LNak2qHBQiUUvJEM8Npc3tg62mg9NWCJ6TVD5SVBtaLWm");
        }

        public void StartFeed()
        {
            IFilteredStream filteredStream = Tweetinvi.Stream.CreateFilteredStream();
            foreach (Term term in _termLogic.GetTerms())
            {
                filteredStream.AddTrack(term.Text);
            }
            filteredStream.MatchingTweetReceived += (sender, args) =>
            {
                UserUpdate newUserUpdate = new UserUpdate()
                {
                    InternalID = Guid.NewGuid(),
                    ExtractionDate = DateTime.Now,
                    Content = args.Tweet.FullText,
                    Source = "Twitter",
                    Term = args.MatchingTracks[0],
                    Location = args.Tweet.CreatedBy.Location
                };
                if (args.Tweet.Coordinates != null)
                {
                    newUserUpdate.Latitude = args.Tweet.Coordinates.Latitude;
                    newUserUpdate.Longitude = args.Tweet.Coordinates.Longitude;
                }
                _userUpdateStorage.Save(newUserUpdate);
            };
            filteredStream.StartStreamMatchingAnyCondition();
            

        }
    }
}
