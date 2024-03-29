﻿using HIS.DAL.DbHelper;
using HIS.Domain.Models.Common;
using HIS.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.DAL.DAL
{
    public class UserDAL : IUserDAL
    {

        #region Initialization

        Database db;
        public IRoleDAL RoleDal { get; set; }

        public UserDAL()
        {
            db = new Database();
            RoleDal = new RoleDAL();
        }

        public UserDAL(Database database)
        {
            db = database;
            RoleDal = new RoleDAL(database);
        }

        #endregion                  

        #region UserType

        public List<UserType> GetUserTypes(SearchCriteria criteria, out int TotalRecords)
        {
            List<UserType> Menus = new List<UserType>();
            try
            {

                TotalRecords = 0;

                DataSet ds = new DataSet();

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_SearchText", Direction = ParameterDirection.Input, Value = criteria.SearchText, Type = DbType.String });
                param.Add(new DbParameter() { Name = "p_Offset", Direction = ParameterDirection.Input, Value = criteria.Offset, Type = DbType.Int32 });
                param.Add(new DbParameter() { Name = "p_PageSize", Direction = ParameterDirection.Input, Value = criteria.PageSize, Type = DbType.Int32 });


                ds = db.LoadDataSetAgainstQuery("pr_GetUserTypes", CommandType.StoredProcedure, ref param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    Menus = ds.Tables[0].AsEnumerable().Select(a => new UserType()
                    {
                        UserTypeId = Convert.ToInt32(a["iUserTypeId"]),
                        UserTypeName = a["vUserTypeName"].ToString()

                    }).ToList();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TotalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Menus;
        }

        public UserType GetUserTypeById(int id)
        {

            UserType userType = new UserType();
            try
            {

                DataSet ds = new DataSet();

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserTypeId", Direction = ParameterDirection.Input, Value = id, Type = DbType.Int32 });


                ds = db.LoadDataSetAgainstQuery("pr_GetUserTypeById", CommandType.StoredProcedure, ref param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    userType = ds.Tables[0].AsEnumerable().Select(a => new UserType()
                    {
                        UserTypeId = Convert.ToInt32(a["iUserTypeId"]),
                        UserTypeName = a["vUserTypeName"].ToString()

                    }).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return userType;


        }



        public bool SaveUserType(UserType userType)
        {
            try
            {

                string query = "pr_SaveUserType";
                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserTypeId", Value = userType.UserTypeId });
                param.Add(new DbParameter() { Name = "p_UserTypeName", Value = userType.UserTypeName });
                param.Add(new DbParameter() { Name = "p_CreatedUserId", Value = userType.CreatedUserId });

                int result = db.ExecuteNonQuery(query, CommandType.StoredProcedure, ref param);


                return result > 0;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int DeleteUserTypeById(int id)
        {
            int deleted = 0;
            try
            {

                string query = "pr_DeleteUserTypeById";
                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_Id", Value = id });

                deleted = db.ExecuteNonQuery(query, CommandType.StoredProcedure, ref param);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return deleted;
        }

        public bool SaveUserAssignedTypes(int userId, List<int> types,int organizationId,int? createdUserId=0)
        {
            bool saved = false;
            try
            {

                DeleteUserAssignedTypes(userId);

                string query = "pr_SaveUserAssignedTypes";

                foreach (int type in types)
                {
                    List<DbParameter> param = new List<DbParameter>();
                    param.Add(new DbParameter() { Name = "p_UserTypeId", Value = type });
                    param.Add(new DbParameter() { Name = "p_OrganizationId", Value = organizationId });
                    param.Add(new DbParameter() { Name = "p_UserId", Value = userId });
                    param.Add(new DbParameter() { Name = "p_CreatedUserId", Value = createdUserId });

                    saved = db.ExecuteNonQuery(query, CommandType.StoredProcedure, ref param) > 0;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return saved;
        }

        private bool DeleteUserAssignedTypes(int userId)
        {
            try
            {

                string query = "pr_DeleteUserAssignedTypes";
                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserId", Value = userId });

                int deleted = db.ExecuteNonQuery(query, CommandType.StoredProcedure, ref param);

                return deleted > 0;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<int> GetUserAssignedTypeIds(int userId)
        {
            List<int> ids = new List<int>();
            try
            {

                DataSet ds = new DataSet();

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserId", Direction = ParameterDirection.Input, Value = userId, Type = DbType.Int32 });


                ds = db.LoadDataSetAgainstQuery("pr_GetTypesAssignedToUser", CommandType.StoredProcedure, ref param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    ids = ds.Tables[0].AsEnumerable().Select(a => Convert.ToInt32(a[0])).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ids;
        }

        #endregion

        #region User

        public List<User> GetUsers(SearchCriteria criteria, out int TotalRecords, int OrganizationId)
        {
            List<User> data = new List<User>();
            try
            {

                TotalRecords = 0;

                DataSet ds = new DataSet();

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_SearchText", Direction = ParameterDirection.Input, Value = criteria.SearchText, Type = DbType.String });
                param.Add(new DbParameter() { Name = "p_Offset", Direction = ParameterDirection.Input, Value = criteria.Offset, Type = DbType.Int32 });
                param.Add(new DbParameter() { Name = "p_PageSize", Direction = ParameterDirection.Input, Value = criteria.PageSize, Type = DbType.Int32 });
                param.Add(new DbParameter() { Name = "p_OrganizationId", Direction = ParameterDirection.Input, Value = OrganizationId, Type = DbType.Int32 });


                ds = db.LoadDataSetAgainstQuery("pr_GetUsers", CommandType.StoredProcedure, ref param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    data = ds.Tables[0].AsEnumerable().Select(a => new User()
                    {
                        UserId = Convert.ToInt32(a["iUserId"]),
                        FirstName = a["vFirstName"].ToString(),
                        LastName = a["vLastName"].ToString(),
                        UserName = a["vUserName"].ToString(),
                        Email = a["vEmail"].ToString(),
                        Designation = a["vDesignation"].ToString()

                    }).ToList();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        TotalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }


        public int SaveUser(User user)
        {
            int saved = 0;

            try
            {

                string query = "pr_SaveUpdateUser";

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_iUserId", Type=DbType.Int32, Value = user.UserId });
                param.Add(new DbParameter() { Name = "p_vUserName", Type=DbType.String,Value = user.UserName });
                param.Add(new DbParameter() { Name = "p_vEmail", Type = DbType.String, Value = user.Email });
                param.Add(new DbParameter() { Name = "p_vFirstName", Type = DbType.String, Value = user.FirstName });
                param.Add(new DbParameter() { Name = "p_vMiddleName", Type = DbType.String, Value = user.MiddleName });
                param.Add(new DbParameter() { Name = "p_vLastName", Type = DbType.String, Value = user.LastName });
                param.Add(new DbParameter() { Name = "p_iCountryId", Type = DbType.Int32, Value = user.CountryId });
                param.Add(new DbParameter() { Name = "p_iCityId", Type = DbType.Int32, Value = user.CityId });
                param.Add(new DbParameter() { Name = "p_iTitleId", Type = DbType.Int32, Value = user.TitleId });
                param.Add(new DbParameter() { Name = "p_iGenderId", Type = DbType.Int32, Value = user.Gender });
                param.Add(new DbParameter() { Name = "p_vPassword", Type = DbType.String, Value = user.Password });
                param.Add(new DbParameter() { Name = "p_vFatherHusbandName", Type = DbType.String, Value = user.FatherHusbandName });
                param.Add(new DbParameter() { Name = "p_bIsFirstTimeLogin", Type = DbType.Boolean, Value = user.FirstTimeLogin });
                param.Add(new DbParameter() { Name = "p_vAddress", Type = DbType.String, Value = user.Address });
                param.Add(new DbParameter() { Name = "p_vPhoneNo", Type = DbType.String, Value = user.PhoneNo });
                param.Add(new DbParameter() { Name = "p_vCNIC", Type = DbType.String, Value = user.CNIC });
                param.Add(new DbParameter() { Name = "p_vPassportNo", Type = DbType.String, Value = user.PassportNo });
                param.Add(new DbParameter() { Name = "p_vEmergencyContactNo", Type = DbType.String, Value = user.EmergencyContactNumber });
                param.Add(new DbParameter() { Name = "p_vEmergencyContactPerson", Type = DbType.String, Value = user.EmergencyContactPerson });
                param.Add(new DbParameter() { Name = "p_iCreatedUserId", Type = DbType.Int32, Value = user.CreatedUserId });
                param.Add(new DbParameter() { Name = "p_iUpdatedUserId", Type = DbType.Int32, Value = user.UpdateUserId });

                param.Add(new DbParameter() { Name = "p_vDesignation", Type = DbType.String, Value = user.Designation });
                param.Add(new DbParameter() { Name = "p_bIsActive", Type = DbType.Boolean, Value = user.IsActive });
                param.Add(new DbParameter() { Name = "p_iOrganizationId", Type = DbType.Int32, Value = user.OrganizationId });


                saved = Convert.ToInt32(db.ExecuteScalar(query, CommandType.StoredProcedure, ref param));



            }
            catch (Exception ex)
            {

                throw ex;
            }

            return saved;
        }


        public bool DoesUsernameOrEmailExists(string username, string email,int? userId=0)
        {
            bool exists = false;

            try
            {

                string query = "pr_GetUsernameEmailCount";

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_Username", Value = username });
                param.Add(new DbParameter() { Name = "p_Email", Value = email });
                param.Add(new DbParameter() { Name = "p_UserId", Value = userId });

                int saved = Convert.ToInt32(db.ExecuteScalar(query, CommandType.StoredProcedure, ref param));

                if (saved > 0)
                {
                    exists = true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return exists;

        }

        public User GetUserById(int id)
        {
            DataSet ds = new DataSet();
            User user = new User();
            try
            {

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserId", Direction = ParameterDirection.Input, Value = id });


                ds = db.LoadDataSetAgainstQuery("pr_GetUserById", CommandType.StoredProcedure, ref param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    user = ds.Tables[0].AsEnumerable().Select(a => new User()
                    {
                        UserId = Convert.ToInt32(a["iUserId"]),
                        FirstName = a["vFirstName"].ToString(),
                        LastName = a["vLastName"].ToString(),
                        UserName = a["vUserName"].ToString(),
                        Email = a["vEmail"].ToString(),
                        Designation = a["vDesignation"].ToString(),
                        TitleId = string.IsNullOrEmpty(a["iTitleId"].ToString()) ?0:Convert.ToInt32(a["iTitleId"]),
                        Gender = string.IsNullOrEmpty(a["iGenderId"].ToString()) ? 0 : Convert.ToInt32(a["iGenderId"]),
                        Password = a["vPassword"].ToString(),
                        FatherHusbandName = a["vfatherhusbandname"].ToString(),
                        FirstTimeLogin = string.IsNullOrEmpty(a["bisfirsttimelogin"].ToString()) ? false : Convert.ToBoolean(a["bisfirsttimelogin"]),
                        Address = a["vaddress"].ToString(),
                        PhoneNo = a["vPhoneNo"].ToString(),
                        CNIC = a["vCNIC"].ToString(),
                        PassportNo = a["vpassportno"].ToString(),
                        EmergencyContactNumber = a["vemergencycontactno"].ToString(),
                        EmergencyContactPerson = a["vemergencycontactperson"].ToString(),
                        IsActive = string.IsNullOrEmpty(a["bIsActive"].ToString()) ? false : Convert.ToBoolean(a["bIsActive"]),
                        OrganizationId = string.IsNullOrEmpty(a["iOrganizationId"].ToString()) ? 0 : Convert.ToInt32(a["iOrganizationId"]),
                        CreatedDate = Convert.ToDateTime(a["dCreatedDate"])

                    }).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return user;
        }

        public bool SaveUserData(User user, List<int> iRoleId, List<int> UserTypeId)
        {
            bool saved = false;
            try
            {
                db.BeginTransaction();
                int userId = SaveUser(user);
                if (userId > 0)
                {
                    saved = SaveUserAssignedTypes(userId, UserTypeId,user.OrganizationId,user.CreatedUserId);
                    saved = SaveUserRole(userId, iRoleId,user.CreatedUserId);
                    db.Commit();
                }
                else
                {
                    db.RollBack();
                }

            }
            catch (Exception ex)
            {
                db.RollBack();
                throw ex;
            }
            return saved;
        }
        public bool DeleteUserById(int id)
        {
            bool deleted = false;
            try
            {

                string query = "pr_DeleteUserById";
                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserId", Value = id });

                deleted = db.ExecuteNonQuery(query, CommandType.StoredProcedure, ref param)>0;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return deleted;
        }

        private bool SaveUserRole(int userId, List<int> Role, int? createdUserId = 0)
        {
            bool saved = false;
            try
            {

                DeleteUserRoles(userId);

                string query = "pr_SaveUserRole";

                foreach (int rol in Role)
                {
                    List<DbParameter> param = new List<DbParameter>();
                    param.Add(new DbParameter() { Name = "p_RoleId", Value = rol });
                    param.Add(new DbParameter() { Name = "p_UserId", Value = userId });
                    param.Add(new DbParameter() { Name = "p_CreatedUserId", Value = createdUserId });

                    saved = db.ExecuteNonQuery(query, CommandType.StoredProcedure, ref param) > 0;
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }
            return saved;
        }

        private bool DeleteUserRoles(int userId)
        {
            try
            {

                string query = "pr_DeleteUserRoles";
                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserId", Value = userId });

                int deleted = db.ExecuteNonQuery(query, CommandType.StoredProcedure, ref param);

                return deleted > 0;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion

        #region Login

        public User LoginUser(string userName, string password)
        {
            DataSet ds = new DataSet();
            User user = new User();
            try
            {

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_userName", Value = userName });
                param.Add(new DbParameter() { Name = "p_Password", Value = password });


                ds = db.LoadDataSetAgainstQuery("pr_ValidateLogin", CommandType.StoredProcedure, ref param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    user = ds.Tables[0].AsEnumerable().Select(a => new User()
                    {
                        UserId = Convert.ToInt32(a["iUserId"]),
                        FirstName = a["vFirstName"].ToString(),
                        LastName = a["vLastName"].ToString(),
                        UserName = a["vUserName"].ToString(),
                        Email = a["vEmail"].ToString(),
                        Designation = a["vDesignation"].ToString(),
                        TitleId = string.IsNullOrEmpty(a["iTitleId"].ToString())?0:Convert.ToInt32(a["iTitleId"]),
                        Gender = string.IsNullOrEmpty(a["iGenderId"].ToString()) ? 0 : Convert.ToInt32(a["iGenderId"]),
                        FatherHusbandName = a["vfatherhusbandname"].ToString(),
                        FirstTimeLogin = string.IsNullOrEmpty(a["bisfirsttimelogin"].ToString()) ? false : Convert.ToBoolean(a["bisfirsttimelogin"]),
                        Address = a["vaddress"].ToString(),
                        PhoneNo = a["vPhoneNo"].ToString(),
                        CNIC = a["vCNIC"].ToString(),
                        PassportNo = a["vpassportno"].ToString(),
                        EmergencyContactNumber = a["vemergencycontactno"].ToString(),
                        EmergencyContactPerson = a["vemergencycontactperson"].ToString(),
                        IsActive = string.IsNullOrEmpty(a["bIsActive"].ToString()) ?false : Convert.ToBoolean(a["bIsActive"]),
                        OrganizationId = string.IsNullOrEmpty(a["iOrganizationId"].ToString()) ? 0 : Convert.ToInt32(a["iOrganizationId"]),

                    }).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return user;
        }

        public List<string> GetUserRights(int userId)
        {
            List<string> rights = new List<string>();
            try
            {

                DataSet ds = new DataSet();

                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_UserId", Direction = ParameterDirection.Input, Value = userId, Type = DbType.Int32 });


                ds = db.LoadDataSetAgainstQuery("pr_GetUserRights", CommandType.StoredProcedure, ref param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    rights = ds.Tables[0].AsEnumerable().Select(a => a[0].ToString()).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return rights;
        }
        #endregion

        #region User Profile


        //public bool UpdateUserProfile(User user)
        //{
        //    bool saved = false;

        //    try
        //    {

        //        string query = "pr_UpdateUserProfile";

        //        List<DbParameter> param = new List<DbParameter>();
        //        param.Add(new DbParameter() { Name = "p_iUserId", Type = DbType.Int32, Value = user.UserId });

        //        param.Add(new DbParameter() { Name = "p_vFirstName", Type = DbType.String, Value = user.FirstName });
        //        param.Add(new DbParameter() { Name = "p_vMiddleName", Type = DbType.String, Value = user.MiddleName });
        //        param.Add(new DbParameter() { Name = "p_vLastName", Type = DbType.String, Value = user.LastName });
        //        param.Add(new DbParameter() { Name = "p_iCountryId", Type = DbType.Int32, Value = user.CountryId });
        //        param.Add(new DbParameter() { Name = "p_iCityId", Type = DbType.Int32, Value = user.CityId });
        //        param.Add(new DbParameter() { Name = "p_iTitleId", Type = DbType.Int32, Value = user.TitleId });
        //        param.Add(new DbParameter() { Name = "p_iGenderId", Type = DbType.Int32, Value = user.Gender });
        //        param.Add(new DbParameter() { Name = "p_vFatherHusbandName", Type = DbType.String, Value = user.FatherHusbandName });
        //        param.Add(new DbParameter() { Name = "p_vAddress", Type = DbType.String, Value = user.Address });
        //        param.Add(new DbParameter() { Name = "p_vPhoneNo", Type = DbType.String, Value = user.PhoneNo });
        //        param.Add(new DbParameter() { Name = "p_vCNIC", Type = DbType.String, Value = user.CNIC });
        //        param.Add(new DbParameter() { Name = "p_vPassportNo", Type = DbType.String, Value = user.PassportNo });
        //        param.Add(new DbParameter() { Name = "p_vEmergencyContactNo", Type = DbType.String, Value = user.EmergencyContactNumber });
        //        param.Add(new DbParameter() { Name = "p_vEmergencyContactPerson", Type = DbType.String, Value = user.EmergencyContactPerson });
        //        param.Add(new DbParameter() { Name = "p_vDesignation", Type = DbType.String, Value = user.Designation });


        //        saved = Convert.ToInt32(db.ExecuteScalar(query, CommandType.StoredProcedure, ref param)) > 0;



        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return saved;
        //}

        public bool UpdateUserProfile(User user)///// my Editing 
        {
            bool Saved = false;
            try
            {

                string query = "UpdateUserProfile1";
                List<DbParameter> param = new List<DbParameter>();
                param.Add(new DbParameter() { Name = "p_iUserId", Type = DbType.Int32, Value = user.UserId });
                param.Add(new DbParameter() {Name="p_vUserName",Type=DbType.String,Value=user.UserName });
                param.Add(new DbParameter() { Name = "p_vEmail", Type = DbType.String, Value = user.Email });
                param.Add(new DbParameter() {Name="p_vFirstName",Type=DbType.String,Value=user.FirstName });
                param.Add(new DbParameter() { Name = "p_vLastName", Type = DbType.String, Value = user.LastName });
                param.Add(new DbParameter() { Name = "p_vFatherHusbandName", Type = DbType.String, Value = user.FatherHusbandName });
                param.Add(new DbParameter() { Name = "p_vAddress", Type = DbType.String, Value = user.Address });
                param.Add(new DbParameter() {Name="p_vPhoneNo",Type=DbType.String,Value=user.PhoneNo});
                param.Add(new DbParameter() { Name = "p_vCNIC",Type=DbType.String,Value=user.CNIC });
                param.Add(new DbParameter() { Name = "p_vEmergencyContactNo", Type = DbType.String, Value = user.EmergencyContactNumber });
                param.Add(new DbParameter() { Name = "p_vUserImage", Type = DbType.String, Value = user.vUserImage });

                Saved = Convert.ToInt32(db.ExecuteScalar(query, CommandType.StoredProcedure, ref param)) > 0;


            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return Saved;

        }

        #endregion

        public User GetDataForUserProfile(int id)
        {
            User user = new User();
            user = GetUserById(id);
            return user;
        }
    }

    public interface IUserDAL
    {
        #region UserType

        List<UserType> GetUserTypes(SearchCriteria criteria, out int TotalRecords);
        UserType GetUserTypeById(int id);
        int DeleteUserTypeById(int id);
        bool SaveUserType(UserType userType);
        bool SaveUserAssignedTypes(int userId, List<int> types, int organizationId, int? createdUserId = 0);
        List<int> GetUserAssignedTypeIds(int userId);

        #endregion

        #region User

        int SaveUser(User user);
        List<User> GetUsers(SearchCriteria criteria, out int TotalRecords, int OrganizationId);
        bool DoesUsernameOrEmailExists(string username, string email,int? userId=0);
        User GetUserById(int id);
        bool SaveUserData(User user, List<int> iRoleId, List<int> UserTypeId);
        bool DeleteUserById(int id);

        #endregion

        #region Login
        User LoginUser(string userName, string password);
        List<string> GetUserRights(int userId);
        #endregion

        #region User profile

        bool UpdateUserProfile(User user);

        //bool NewUpdateUserProfile(User user);//////// My Editing 
        User GetDataForUserProfile(int id);

        #endregion
    }
}
