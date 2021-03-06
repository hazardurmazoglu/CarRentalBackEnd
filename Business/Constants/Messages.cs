using Core.Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string AuthorizationDenied = "Authorization denied.";
        public static string UserRegistered = "Successfully Registered.";
        public static string UserNotFound = "User not found.";
        public static string PasswordError = "Wrong Password.";
        public static string SuccessfulLogin = "Successful login.";
        public static string UserAlreadyExists = "User already exists.";
        public static string AccessTokenCreated = "Welcome";
        public static string CarImageLimitExceeded = "Car image limit exceeded.";
        public static string NoCar = "There is no car.";
        public static string CardExist= "Card already exists.";
        public static string NotCarAvailable = "Car is not available.";
        public static string NotEnough = "Your findex score is not enough." ;
        public static string RentalNotAvailable = "Car is not available.";
    }
}