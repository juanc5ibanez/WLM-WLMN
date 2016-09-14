using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLM_WLMN.Common.DTOs;
using WLM_WLMN.Common.Interfaces;
using WLM_WLMN.Logic.Terms;
using WLM_WLMN.CassandraStorage;
using Stream = WLM_WLMN.Logic.Twitter.Stream;

namespace WLM_WLMN.TwitterFeeder
{
    class Program
    {
        private TermStorage _termStorage;
        private TermLogic _termLogic;
        private ITwitterLogic _twitterStreamLogic;
        private UserUpdateStorage _userUpdateStorage;

        public Program()
        {
            _termStorage = new TermStorage();
            _termLogic = new TermLogic(_termStorage);
            _userUpdateStorage = new UserUpdateStorage();
            _twitterStreamLogic = new Stream(_termLogic, _userUpdateStorage);   
            //_userUpdateStorage.Backup(@"F:\Development\QUT\Project1\Backups",50000);
            //_userUpdateStorage.Restore(new DirectoryInfo(@"F:\Development\QUT\Project1\Backups"));
        }

        private void StartFeed()
        {
            _twitterStreamLogic.StartFeed();
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            program.StartFeed();
            //Console.ReadLine();
        }
    }
}
