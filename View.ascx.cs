/*
' Copyright (c) 2014  Plugghest.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Web.UI.WebControls;
using Plugghest.Modules.ActionFormReceiver.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using System.Collections.Specialized;
using Plugghest.Base2;


namespace Plugghest.Modules.ActionFormReceiver
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from ActionFormReceiverModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : PortalModuleBase, IActionable
    {
        public NameValueCollection nvc;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                nvc = Request.Form;
                switch (nvc["FormType"])
                {
                    case "CreatePlugg":
                        CreatePlugg();
                        break;
                    case "CreateCourse":
                        CreateCourse();
                        break;
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void CreatePlugg()
        {
            string cultureCode = (Page as DotNetNuke.Framework.PageBase).PageCulture.Name;
            PluggContainer pc = new PluggContainer(cultureCode);
            pc.ThePlugg.CreatedByUserId = UserId;
            pc.ThePlugg.ModifiedByUserId = UserId;
            pc.ThePlugg.PluggId = 0;
            pc.ThePlugg.CreatedInCultureCode = cultureCode;
            pc.SetTitle(nvc["Title"]);
            //string subjectStr = Page.Request.QueryString["s"];
            //if (subjectStr != null)
            //{
            //    int subid = Convert.ToInt32(subjectStr);
            //    pc.ThePlugg.SubjectId = subid;
            //}
            //else
            //    pc.ThePlugg.SubjectId = 0;
            pc.ThePlugg.SubjectId = 0;
            if (nvc["Description"] != "")
                pc.SetDescription(nvc["Description"]);
            switch (nvc["Whocanedit"])
            {
                case "Only me":
                    pc.ThePlugg.WhoCanEdit = EWhoCanEdit.OnlyMe;
                    break;
                default:
                    pc.ThePlugg.WhoCanEdit = EWhoCanEdit.Anyone ;
                    break;
            }
            BaseHandler bh = new BaseHandler();
            bh.CreateBasicPlugg(pc);
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(pc.ThePlugg.TabId, "", "edit=0"));
        }

        private void CreateCourse()
        {
            string cultureCode = (Page as DotNetNuke.Framework.PageBase).PageCulture.Name;
            CourseContainer cc = new CourseContainer(cultureCode);
            cc.TheCourse.CreatedByUserId = UserId;
            cc.TheCourse.ModifiedByUserId = UserId;
            cc.TheCourse.CourseId = 0;
            cc.TheCourse.CreatedInCultureCode = cultureCode;
            cc.SetTitle(nvc["Title"]);
            //string subjectStr = Page.Request.QueryString["s"];
            //if (subjectStr != null)
            //{
            //    int subid = Convert.ToInt32(subjectStr);
            //    pc.ThePlugg.SubjectId = subid;
            //}
            //else
            //    pc.ThePlugg.SubjectId = 0;
            cc.TheCourse.SubjectId = 0;
            if (nvc["Description"] != "")
                cc.SetDescription(nvc["Description"]);
            switch (nvc["Whocanedit"])
            {
                case "Only me":
                    cc.TheCourse.WhoCanEdit = EWhoCanEdit.OnlyMe;
                    break;
                default:
                    cc.TheCourse.WhoCanEdit = EWhoCanEdit.Anyone;
                    break;
            }
            BaseHandler bh = new BaseHandler();
            bh.CreateCourse(cc);
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(cc.TheCourse.TabId, "", "edit=0"));
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }
    }
}