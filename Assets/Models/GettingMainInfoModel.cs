using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Models {
    public class GettingMainInfoModel {
        public MainModel GameModel { get; set; }
        public UserModel RealUser { get; set; }
        public List<UserModel> FakeUsers { get; set; }
    }

    public class GetSessionsInfoModel {
        public Button Button { get; set; }
        public int SessionId { get; set; }
    }
}
