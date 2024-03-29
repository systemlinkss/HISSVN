﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIS.Domain.Models.User;
using HIS.Web.Models;
using HIS.Domain.Models.Common;
using HIS.BLL;
using HIS.Domain.Models.Module;
using HIS.Web.Filters;

namespace HIS.Web.Controllers
{
    public class ModuleController : Controller
    {
        #region Initialization

        public IModuleBll _module { get; set; }

        public ModuleController()
        {
            _module = new ModuleBLL();
        }

        #endregion

        #region Module
        [CheckUserRights]
        public ActionResult ModuleList(string searchText, int offset = 0, int pageSize = 10)
        {
            List<Module> types = new List<Module>();

            SearchCriteria criteria = new SearchCriteria()
            {
                Offset = offset,
                SearchText = searchText ?? "",
                PageSize = pageSize
            };

            int TotalRecords = 0;

            types = _module.GetModule(criteria, out TotalRecords);

            ViewBag.Offset = offset;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = TotalRecords;

            return View(types);
        }
        [CheckUserRights]
        [HttpGet]
        public ActionResult ModuleForm(int id = 0)
        {
            Module module = new Module();
            ViewBag.CoreModule = _module.GetCoreModuleDrpData().AsEnumerable().Select(a => new SelectListItem()
            {
                Text = a.CoreModuleText,
                Value = a.CoreModuleId.ToString()
            }).ToList();

            if (id > 0)
            {
                module = _module.GetModuleById(id);
                // get previous record for editing
            }

            return View(module);
        }
        [CheckUserRights]
        [HttpPost]
        [ValidateAjax]
        public ActionResult ModuleForm(Module module)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxableViewResult(module);
            }
            // TODO : assign user id from session to created user id..
            module.CreatedUserId = Helper.GetLoggedInUserId();
            bool saved = _module.SaveModule(module);

            AjaxResponse response = new AjaxResponse()
            {
                Type = saved ? "success" : "error",
                Message = saved ? "Data has been processed successfully" : "Unable to process data",
                Heading = "Module"         ,
                RedirectUrl = Url.Action("ModuleList","Module")

            };

            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteModuleById(int id)
        {
            string message = "";
            string mclass = "";
            int deleted = _module.DeleteModuleById(id);
            if (deleted > 0)
            {
                message = "Operation completed successfully";
                mclass = "info";
            }
            else
            {
                message = "Unable to delete Module";
                mclass = "error";
            }

            AjaxResponse res = new AjaxResponse()
            {
                Message = message,
                Type = mclass,
                Heading = "Module"                ,
                RedirectUrl = Url.Action("ModuleList", "Module")
            };


            return Json(res);
        }

        #endregion

        #region Module Permission
        [CheckUserRights]
        public ActionResult ModulePermissionList(string searchText, int offset = 0, int pageSize = 10)
        {
            List<ModulePermission> types = new List<ModulePermission>();

            SearchCriteria criteria = new SearchCriteria()
            {
                Offset = offset,
                SearchText = searchText ?? "",
                PageSize = pageSize
            };

            int TotalRecords = 0;

            types = _module.GetModulePermission(criteria, out TotalRecords);

            ViewBag.Offset = offset;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalRecords = TotalRecords;

            return View(types);
        }
        [CheckUserRights]
        [HttpGet]
        public ActionResult ModulePermissionForm(int id = 0)
        {
            ModulePermission modulePermission = new ModulePermission();
            int total = 0;
            ViewBag.Modules = _module.GetModule(new SearchCriteria() { Offset = 0, PageSize = 500, SearchText = "" }, out total)
            .Where(a => a.OrganizationId == 0 || a.OrganizationId == Helper.GetLoggedInUserOrganization()).AsEnumerable().Select(a => new SelectListItem()
            {
                Text = a.ModuleName,
                Value = a.ModuleId.ToString()
            }).ToList();

            if (id > 0)
            {
                modulePermission = _module.GetModulePermissionById(id);
                // get previous record for editing
            }

            return View(modulePermission);
        }
        [CheckUserRights]
        [HttpPost]
        [ValidateAjax]
        public ActionResult ModulePermissionForm(ModulePermission modulePermission)
        {
            if (!ModelState.IsValid)
            {
                return new AjaxableViewResult(modulePermission);
            }
            // TODO : assign user id from session to created user id..
            bool saved = _module.SaveModulePermission(modulePermission);

            AjaxResponse response = new AjaxResponse()
            {
                Type = saved ? "success" : "error",
                Message = saved ? "Data has been processed successfully" : "Unable to process data",
                Heading = "Module Permission",
                RedirectUrl = Url.Action("ModulePermissionList","Module")

            };

            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteModulePermissionById(int id)
        {
            string message = "";
            string mclass = "";
            int deleted = _module.DeleteModulePermissionById(id);
            if (deleted > 0)
            {
                message = "Operation completed successfully";
                mclass = "info";
            }
            else
            {
                message = "Unable to delete Module Permission";
                mclass = "error";
            }

            AjaxResponse res = new AjaxResponse()
            {
                Message = message,
                Type = mclass,
                Heading = "Module Permission" ,
                RedirectUrl = Url.Action("ModulePermissionList", "Module")
            };


            return Json(res);
        }

        #endregion

    }
}
