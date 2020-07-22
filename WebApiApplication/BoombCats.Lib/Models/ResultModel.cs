using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoombCats.Lib.Models {
    // модель для результата операций
    public class ResultModel {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("status")]
        public bool Status { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
        // на случай ошибки
        public ResultModel(bool status, string error) {
            Error = error;
            Status = status;
        }
        // на случай успеха
        public ResultModel(bool status, object data) {
            Status = status;
            Data = data;
        }
        // н случай, если надо будет просто подтвердить или опровергнуть выполнение действия
        public ResultModel(bool status) {
            Status = status;
        }

    }
}