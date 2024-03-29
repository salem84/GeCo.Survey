﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMembership;
using MvcMembership.Settings;
using Microsoft.Practices.Unity;
using System.Net.Mail;
using System.Web.Security;
using GeCoSurvey.Web.Areas.Admin.Models.Users;
using PagedList;

namespace GeCoSurvey.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class UsersController : Controller
    {
        //
        // GET: /Prova/UserAdministration/

        private const int PageSize = 10;
        private const string ResetPasswordBody = "Your new password is: ";
        private const string ResetPasswordFromAddress = "from@domain.com";
        private const string ResetPasswordSubject = "Your New Password";
        private readonly IRolesService _rolesService;
        private readonly ISmtpClient _smtpClient;
        private readonly IMembershipSettings _membershipSettings;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly GeCoSurvey.Service.IUserService _userServiceGeco;

        [InjectionConstructor]
        public UsersController(GeCoSurvey.Service.IUserService userServiceGeco)
            : this(new AspNetMembershipProviderWrapper(), new AspNetRoleProviderWrapper(), new SmtpClientProxy())
        {
            _userServiceGeco = userServiceGeco;
        }

        public UsersController(AspNetMembershipProviderWrapper membership, IRolesService roles, ISmtpClient smtp)
            : this(membership.Settings, membership, membership, roles, smtp)
        {
        }

        public UsersController(
            IMembershipSettings membershipSettings,
            IUserService userService,
            IPasswordService passwordService,
            IRolesService rolesService,
            ISmtpClient smtpClient)
        {
            _membershipSettings = membershipSettings;
            _userService = userService;
            _passwordService = passwordService;
            _rolesService = rolesService;
            _smtpClient = smtpClient;
        }

        

        public ViewResult Index(int? page, string nome, string cognome)
        {
            if (!string.IsNullOrWhiteSpace(nome) || !string.IsNullOrWhiteSpace(cognome))
            {
                // prendo tutti gli utenti
                int count = _userService.TotalUsers;
                var totalUsers = _userService.FindAll(0, count);

                // mi leggo i profili
                var profiles = from user in totalUsers
                               select new
                               {
                                   profilo = _userServiceGeco.GetUtente(user.UserName),
                                   user = user
                               };

                var filteredUsers = profiles;

                // controllo se è stata inserita la stringa di filtro per la ricerca del nome
                if (!string.IsNullOrWhiteSpace(nome))
                {
                    filteredUsers = from u in filteredUsers
                                    where u.profilo.Nome.ToLower().IndexOf(nome.ToLower()) != -1
                                    select u;
                }

                // controllo se è stata inserita la stringa di filtro per la ricerca del cognome
                if (!string.IsNullOrWhiteSpace(cognome))
                {
                    filteredUsers = from u in filteredUsers
                                    where u.profilo.Cognome.ToLower().IndexOf(cognome.ToLower()) != -1
                                    select u;
                }

                // estraggo gli utenti
                var users = filteredUsers.Select(x => x.user);

                int pagesize = Math.Max(users.Count(), 1);
                // faccio questa trasformazione in modo da riutilizzare la stessa vista
                return View(new IndexViewModel
                {
                    Users = new PagedList<MembershipUser>(users, 0, pagesize),
                    Roles = _rolesService.Enabled
                        ? _rolesService.FindAll()
                        : Enumerable.Empty<string>(),
                    IsRolesEnabled = _rolesService.Enabled
                });
            }

            //Non ho fatto nessun filtro, ritorno i risultati paginati

            return View(new IndexViewModel
            {
                Users = _userService.FindAll(page ?? 0, PageSize),
                Roles = _rolesService.Enabled
                    ? _rolesService.FindAll()
                    : Enumerable.Empty<string>(),
                IsRolesEnabled = _rolesService.Enabled
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult CreateRole(string id)
        {
            if (_rolesService.Enabled)
                _rolesService.Create(id);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult DeleteRole(string id)
        {
            _rolesService.Delete(id);
            return RedirectToAction("Index");
        }

        public ViewResult Role(string id)
        {
            var model = new RoleViewModel
            {
                Role = id,
                Users = _rolesService.FindUserNamesByRole(id).Select(username => _userService.Get(username))
            };
            
            return View(model);
        }

        public ViewResult Details(Guid id)
        {
            var user = _userService.Get(id);
            var userRoles = _rolesService.Enabled
                ? _rolesService.FindByUser(user)
                : Enumerable.Empty<string>();
            return View(new DetailsViewModel
            {
                CanResetPassword = _membershipSettings.Password.ResetOrRetrieval.CanReset,
                RequirePasswordQuestionAnswerToResetPassword = _membershipSettings.Password.ResetOrRetrieval.RequiresQuestionAndAnswer,
                DisplayName = user.UserName,
                User = user,
                Roles = _rolesService.Enabled
                    ? _rolesService.FindAll().ToDictionary(role => role, role => userRoles.Contains(role))
                    : new Dictionary<string, bool>(0),
                IsRolesEnabled = _rolesService.Enabled,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Unapproved
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });
        }

        public ViewResult Password(Guid id)
        {
            var user = _userService.Get(id);
            var userRoles = _rolesService.Enabled
                ? _rolesService.FindByUser(user)
                : Enumerable.Empty<string>();
            return View(new DetailsViewModel
            {
                CanResetPassword = _membershipSettings.Password.ResetOrRetrieval.CanReset,
                RequirePasswordQuestionAnswerToResetPassword = _membershipSettings.Password.ResetOrRetrieval.RequiresQuestionAndAnswer,
                DisplayName = user.UserName,
                User = user,
                Roles = _rolesService.Enabled
                    ? _rolesService.FindAll().ToDictionary(role => role, role => userRoles.Contains(role))
                    : new Dictionary<string, bool>(0),
                IsRolesEnabled = _rolesService.Enabled,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Unapproved
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });
        }

        public ViewResult UsersRoles(Guid id)
        {
            var user = _userService.Get(id);
            var userRoles = _rolesService.FindByUser(user);
            return View(new DetailsViewModel
            {
                CanResetPassword = _membershipSettings.Password.ResetOrRetrieval.CanReset,
                RequirePasswordQuestionAnswerToResetPassword = _membershipSettings.Password.ResetOrRetrieval.RequiresQuestionAndAnswer,
                DisplayName = user.UserName,
                User = user,
                Roles = _rolesService.FindAll().ToDictionary(role => role, role => userRoles.Contains(role)),
                IsRolesEnabled = true,
                Status = user.IsOnline
                            ? DetailsViewModel.StatusEnum.Online
                            : !user.IsApproved
                                ? DetailsViewModel.StatusEnum.Unapproved
                                : user.IsLockedOut
                                    ? DetailsViewModel.StatusEnum.LockedOut
                                    : DetailsViewModel.StatusEnum.Offline
            });
        }

        public ViewResult CreateUser()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateUser(CreateUserViewModel createUserViewModel)
        {
            if (!ModelState.IsValid)
                return View(createUserViewModel);

            try
            {
                var user = _userService.Create(
                    createUserViewModel.Username,
                    createUserViewModel.Password,
                    createUserViewModel.Email,
                    createUserViewModel.PasswordQuestion,
                    createUserViewModel.PasswordAnswer,
                    true);
                return RedirectToAction("Details", new { id = user.ProviderUserKey });
            }
            catch (MembershipCreateUserException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(createUserViewModel);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult Details(Guid id, string email, string comments)
        {
            var user = _userService.Get(id);
            user.Email = email;
            user.Comment = comments;
            _userService.Update(user);
            return RedirectToAction("Details", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult DeleteUser(Guid id)
        {
            _userService.Delete(_userService.Get(id));
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ChangeApproval(Guid id, bool isApproved)
        {
            var user = _userService.Get(id);
            user.IsApproved = isApproved;
            _userService.Update(user);
            return RedirectToAction("Details", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult Unlock(Guid id)
        {
            _passwordService.Unlock(_userService.Get(id));
            return RedirectToAction("Details", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ResetPassword(Guid id)
        {
            var user = _userService.Get(id);
            var newPassword = _passwordService.ResetPassword(user);

            var body = ResetPasswordBody + newPassword;
            _smtpClient.Send(new MailMessage(ResetPasswordFromAddress, user.Email, ResetPasswordSubject, body));

            return RedirectToAction("Password", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult ResetPasswordWithAnswer(Guid id, string answer)
        {
            var user = _userService.Get(id);
            var newPassword = _passwordService.ResetPassword(user, answer);

            var body = ResetPasswordBody + newPassword;
            _smtpClient.Send(new MailMessage(ResetPasswordFromAddress, user.Email, ResetPasswordSubject, body));

            return RedirectToAction("Password", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult SetPassword(Guid id, string password)
        {
            var user = _userService.Get(id);
            _passwordService.ChangePassword(user, password);

            var body = ResetPasswordBody + password;
            _smtpClient.Send(new MailMessage(ResetPasswordFromAddress, user.Email, ResetPasswordSubject, body));

            return RedirectToAction("Password", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult AddToRole(Guid id, string role)
        {
            _rolesService.AddToRole(_userService.Get(id), role);
            return RedirectToAction("UsersRoles", new { id });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public RedirectToRouteResult RemoveFromRole(Guid id, string role)
        {
            _rolesService.RemoveFromRole(_userService.Get(id), role);
            return RedirectToAction("UsersRoles", new { id });
        }


        #region PROFILO
        [HttpGet]
        public ActionResult Profile(string id)
        {
            //GetUtente(string username)
            GeCoSurvey.Service.IUserProperties props = _userServiceGeco.GetUtente(id);

            //Popolo ProfileViewModel
            var profileVM = new ProfileViewModel();
            profileVM.Username = id;
            profileVM.Matricola = props.Matricola;
            profileVM.Nome = props.Nome;
            profileVM.Cognome = props.Cognome;
            profileVM.Area = props.Area;
            
            return View(profileVM);
        }


        [HttpPost]
        public ActionResult Profile(string id, ProfileViewModel profilo)
        {
            /*GeCoSurvey.Service.UserProfile profilo = new GeCoSurvey.Service.UserProfile();
            profilo.Nome = form["Nome"];
            profilo.Cognome = form["Cognome"];
            profilo.Area = form["Area"];
            profilo.Matricola = form["Matricola"];*/

            _userServiceGeco.SalvaProfilo(id, profilo);

            return RedirectToAction("Index");
        }

        #endregion

    }
}
