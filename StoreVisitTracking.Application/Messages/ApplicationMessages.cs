using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.Messages
{
    class ApplicationMessages
    {
        // Auth
        public const string ThisUsernameIsAlreadyInUse = "Bu kullanıcı adı zaten kullanılıyor";
        public const string UsernameOrPasswordIsWrong = "Kullanıcı adı veya şifre hatalı";
        public const string UnauthorizedAccess = "Yetkiniz yok";

        // Store
        public const string StoreNotFound = "Mağaza bulunamadı";

        // Visit
        public const string VisitNotFound = "Visit bulunamadı";

        // Paginate
        public const string PageIndexMustBeGreaterThanOrEqualToZero = "Sayfa indeksi sıfır veya daha büyük olmalıdır";
        public const string PageSizeMustBeGreaterThanZero = "Sayfa boyutu sıfırdan daha büyük olmalıdır";

    }
}
