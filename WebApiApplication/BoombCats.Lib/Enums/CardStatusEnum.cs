using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Enums {
    public enum CardStatusEnum : int{
        /// <summary>
        /// карта у одного из пользователей
        /// </summary>
        Active = 0,
        /// <summary>
        /// свободная карта
        /// </summary>
        Free = 1,
        /// <summary>
        /// карта в сбросе
        /// </summary>
        Used = 2,
        /// <summary>
        /// карта в игре - отображается в центре экрана
        /// </summary>
        InGame = 3
    }
}
