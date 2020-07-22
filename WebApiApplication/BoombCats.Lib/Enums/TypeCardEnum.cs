using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoombCats.Lib.Enums {
    public enum TypeCardEnum : int {
        /// <summary>
        /// взрывные котята
        /// </summary>
        Boomb = 1,
        /// <summary>
        /// обезвредить
        /// </summary>
        Neutralize = 2,
        /// <summary>
        /// нет
        /// </summary>
        No = 3,
        /// <summary>
        /// атаковать
        /// </summary>
        Attack = 4,
        /// <summary>
        /// пропустить
        /// </summary>
        Skip = 5,
        /// <summary>
        /// одолжение
        /// </summary>
        Lending = 6,
        /// <summary>
        /// перемешать
        /// </summary>
        Mix = 7,
        /// <summary>
        /// заглянуть в будущее
        /// </summary>
        LookInFuture = 8,
        /// <summary>
        /// карты кошек первого типа
        /// </summary>
        CatCardOne = 9,
        /// <summary>
        /// карты кошек второго типа
        /// </summary>
        CatCardTwo = 10,
        /// <summary>
        /// карты кошек третьего типа
        /// </summary>
        CatCardThree = 11,
        /// <summary>
        /// карты кошек четвертого типа
        /// </summary>
        CatCardFour = 12,
        /// <summary>
        /// карты кошек пятого типа
        /// </summary>
        CatCardFive = 13
    }
}
