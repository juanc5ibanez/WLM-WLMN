using System.Collections;
using System.Collections.Generic;
using Antlr.Runtime.Misc;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Owin;
using Owin;
using WLM_WLMN.Web.Models;

[assembly: OwinStartupAttribute(typeof(WLM_WLMN.Web.Startup))]
namespace WLM_WLMN.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Term, Common.DTOs.Term>();
                cfg.CreateMap<Common.DTOs.Term, Term>();
            });
        }

    }

}
