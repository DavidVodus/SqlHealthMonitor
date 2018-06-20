using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlHealthMonitor.BLL.Models.WebPages;

using SqlHealthMonitor.DAL.Models.WebPages;

using SqlHealthMonitor.DAL.Repositories;

namespace SqlHealthMonitor.BLL.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using System.Collections.Generic;


        public class PageService : ServiceBase, IPageService
        {

            private MapperConfiguration _config;


            private IPageRepository _pageRep;

            public PageService(IPageRepository pageRep)
            {

                _pageRep = pageRep;
                _config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<PageBase, PageSettingViewModel>().ForMember(y => y.PageType, opt =>
                        opt.MapFrom(o => o.GetType().Name));

                    cfg.CreateMissingTypeMaps = true;
                });
            }

            protected override Type LogPrefix => GetType();

            public List<PageSettingViewModel> ReadPages(string userId)
            {
            if (userId == null)
                throw new Exception("userId is null");
            var mapper = new Mapper(_config);
                var page = _pageRep.GetQueryable()
                    .Where(o =>  o.ApplicationUserId == userId);
           
            var pageViewModel = mapper.DefaultContext.Mapper.Map<List<PageSettingViewModel>>(page);
                return pageViewModel;
            }

            public void SaveUserPreferences(PageBase page)
            {
                _pageRep.Update(page);
                _pageRep.Save();

            }
        }
    }


