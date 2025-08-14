using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Presentation.Helpers; // Viewde kullanıcaksak bu namespace'in bulunması zorunlu,yoksa import olmaz.

public static class SessionExtensions //static class olma nedeni nesne oluşturmadan direkt kullanmak için
{
    public static void SetObject<T>(this ISession session, string key, T value) //session içine generic tipte veri eklemek için 
    {
        var jsonData_ = JsonConvert.SerializeObject(value); //Json stringe çevriliyor
        session.SetString(key, jsonData_); //json stringi sessiona koyuyor
    }

    public static T? GetObject<T>(this ISession session, string key) // T? olarak kullanılmazsa null dönme uyarısı gelir
    {
        var jsonData = session.GetString(key); //sessiondan string veriyi alıyor
        if (jsonData == null) //null kontrolü 
            return default(T);
        //default() ise T tipinin(her neyse) default değerini döndürür,int için 0,string için null,bool için false

        return JsonConvert.DeserializeObject<T>(jsonData); //jsonstringi nesneye çeviriyor
    }
}
