using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GorselOdev3.Models;

namespace GorselOdev3.Services
{
    public class FirebaseService
    {
        private const string FirebaseUrl = "https://gorselodev3-8be94-default-rtdb.firebaseio.com/";

        private readonly FirebaseClient _firebase;

        public FirebaseService()
        {
            _firebase = new FirebaseClient(FirebaseUrl);
        }

        public async Task<List<Yapilacak>> GetYapilacaklarAsync()
        {
            return (await _firebase
                .Child("Yapilacaklar")
                .OnceAsync<Yapilacak>())
                .Select(item => new Yapilacak
                {
                    Id = item.Key,
                    Baslik = item.Object.Baslik,
                    Aciklama = item.Object.Aciklama,
                    OlusturmaTarihi = item.Object.OlusturmaTarihi,
                    Tamamlandi = item.Object.Tamamlandi
                })
                .ToList();
        }

        public async Task AddYapilacakAsync(Yapilacak yapilacak)
        {
            await _firebase
                .Child("Yapilacaklar")
                .PostAsync(yapilacak);
        }

        public async Task UpdateYapilacakAsync(Yapilacak yapilacak)
        {
            await _firebase
                .Child("Yapilacaklar")
                .Child(yapilacak.Id)
                .PutAsync(yapilacak);
        }

        public async Task DeleteYapilacakAsync(string id)
        {
            await _firebase
                .Child("Yapilacaklar")
                .Child(id)
                .DeleteAsync();
        }
    }
}