using FirebaseAdmin.Auth;
using System;
using System.Threading.Tasks;
using szosztar.Data.Interfaces;
using szosztar.Logic.Interfaces;

namespace szosztar.Logic
{
    public class AuthLogic : IAuthLogic
    {
        private readonly IDataAccess dataAccess;

        public AuthLogic(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<string> FirebaseAuthenticate(string authToken)
        {
            var firebaseUser = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(authToken);
            var userId = firebaseUser?.Uid;

            return userId;
        }
    }
}
