using System;
using System.Text.RegularExpressions;

using TTechIMCore.DBAccess;

/*
 * TTech IM - Data-Business Transition for App Users
 * 
 * Noah Tong - Jan 05, 2017
 * */

namespace TTechIMCore.UIClasses
{
    public class UIUser : User
    {
        /* Username Spec.
         * 3 to 15 characters with any lower case character, digit or special symbol "_", "-" only
         * */
        public const string UsernameRegexString = @"^[a-z0-9_-]{3,15}$";

        /* Password Spec.
         * 1. Makes sure there are no white-space characters
         * 2. Minimum length of 8
         * 3. Makes sure there is at least:
         *      one non-alpha character
         *      one upper case character
         *      one lower case character
         * */
        public const string PasswordRegexString = @"^(?=.*[^a-zA-Z])(?=.*[a-z])(?=.*[A-Z])\S{8,}$";
        public const string EmailRegexString = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        public const string PostalCodeRegexString = @"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$)";

        public string RetryPassword { get; set; }

        public static bool IsValidUsername(string value)
        {
            return Regex.IsMatch(value, UIUser.UsernameRegexString);
        }

        public static bool IsValidPassword(string value)
        {
            return Regex.IsMatch(value, UIUser.PasswordRegexString);
        }

        public static bool IsValidEmail(string value)
        {
            return Regex.IsMatch(value, UIUser.EmailRegexString);
        }

        public static bool IsValidPostalCode(string value)
        {
            return Regex.IsMatch(value, UIUser.PostalCodeRegexString);
        }

        public UIUser()
            : base() { }

        public UIUser(UIUser usr)
            : base()
        {
            this.UserId = usr.UserId;
            this.UserName = usr.UserName;
            this.Email = usr.Email;
            this.FirstName = usr.FirstName;
            this.LastName = usr.LastName;
            this.Address = usr.Address;
            this.PostalCode = usr.PostalCode;
            this.Gender = usr.Gender;
            this.IsActive = usr.IsActive;
            this.Password = usr.Password;
            this.SecurityToken = usr.SecurityToken;
            this.Description = usr.Description;
            this.ProfilePhotoUrl = usr.ProfilePhotoUrl;
            this.DBInstance = usr.DBInstance;
            this.DefaultController = usr.DefaultController;
            this.DefaultView = usr.DefaultView;
        }

        public void Assign(User usr)
        {
            this.UserId = usr.UserId;
            this.UserName = usr.UserName;
            this.Email = usr.Email;
            this.FirstName = usr.FirstName;
            this.LastName = usr.LastName;
            this.Address = usr.Address;
            this.PostalCode = usr.PostalCode;
            this.Gender = usr.Gender;
            this.IsActive = usr.IsActive;
            this.Password = usr.Password;
            this.SecurityToken = usr.SecurityToken;
            this.Description = usr.Description;
            this.ProfilePhotoUrl = usr.ProfilePhotoUrl;
            this.DBInstance = usr.DBInstance;
            this.DefaultController = usr.DefaultController;
            this.DefaultView = usr.DefaultView;
        }

        public bool Compare(UIUser usr)
        {
            return this.Email == usr.Email &&
                this.FirstName == usr.FirstName &&
                this.LastName == usr.LastName &&
                this.Address == usr.Address &&
                this.PostalCode == usr.PostalCode &&
                this.Gender == usr.Gender &&
                this.Description == usr.Description;
        }
    }
}
