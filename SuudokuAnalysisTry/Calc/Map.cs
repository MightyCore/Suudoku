using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace SuudokuAnalysisTry.Calc
{
    /// <summary>
    /// 表
    /// </summary>
    [Serializable]
    public static class Map
    {
        #region Field
        public static List<Cell> Cells = new List<Cell>();
        //public static List<Remain> Remains = new List<Remain>();
        public static bool Reset = false;
        #endregion

        #region Class
        /// <summary>
        /// Cell
        /// </summary>
        [Serializable]
        public partial class Cell
        {
            public int Row { get; private set; }
            public int Col { get; private set; }
            public int Area { get; private set; }
            public int Num { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="vIndex"></param>
            public Cell(int vIndex)
            {
                Row = (int)(Math.Floor((decimal)(vIndex / 9)) + 1);
                Col = vIndex - (Row - 1) * 9 + 1;
                Area = (int)(Math.Floor((decimal)(Row - 1) / 3) * 3 + Math.Floor((decimal)((Col - 1) / 3)) + 1);
            }
        }

        /// <summary>
        /// 各数値のカウント
        /// </summary>
        [Serializable]


        public class Remain
        {
            public int Num { get; set; }
            public int Cnt { get; set; }
        }
        #endregion

        #region Property
        /// <summary>
        /// 残りのターゲットを空セルの少ない順に取得
        /// </summary>
        /// <returns></returns>
        public static List<Remain> Targets => Remains.Where(x => x.Cnt < 9).OrderByDescending(x => x.Cnt).ToList();

        public static List<Remain> Remains => Cells.GroupBy(x => x.Num).ToDictionary(x => x.Key, x => x.ToList()).Select(x => new Remain { Num = x.Key, Cnt = x.Value.Count }).ToList();
        #endregion

        #region Initialize
        /// <summary>
        /// 表の配列作成
        /// </summary>
        public static void Initialize()
        {
            Enumerable.Range(0, 9 * 9).ToList().ForEach(i => Cells.Add(new Cell(i)));
            //Enumerable.Range(1, 9).ToList().ForEach(i => Remains.Add(new Remain { Num = i }));
        }
        #endregion

        #region Func
        /// <summary>
        /// 値の格納
        /// </summary>
        /// <param name="vNumArray"></param>
        /// <param name="vRow"></param>
        public static void SetMap(string[] vNumArray, int vRow)
        {
            Cells.Where(x => x.Row == vRow).ToList().ForEach(x => x.SetNum(int.Parse(vNumArray[x.Col - 1])));
        }

        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="vCell"></param>
        /// <param name="vNum"></param>
        public static void SetNum(this Cell vCell, int vNum)
        {
            bool IsReset = vNum > 0 && vCell.Exists().Any(x => x == vNum);
            //if (vCell.Num > 0) Remains.Find(x => x.Num == vCell.Num).Cnt--;
            vCell.Num = vNum;
            if (vNum == 0) return;
            //Remains.Find(x => x.Num == vNum).Cnt++;
            if (IsReset) Reset = true;
        }

        /// <summary>
        /// 値の初期化
        /// </summary>
        public static void ClearNum()
        {
            Cells.ForEach(x => x.Num = 0);
            //Remains.ForEach(x => x.Cnt = 0);
        }

        /// <summary>
        /// 指定番号が存在する行、列、表以外の値が0のセル
        /// </summary>
        /// <param name="vNum"></param>
        /// <returns></returns>
        public static List<Cell> GetZeroCellsWithoutNum(int vNum)
        {
            var wWithouts = Cells.Where(x => x.Num == vNum).ToList();
            return Cells.Where(x => wWithouts.All(y => y.Row != x.Row && y.Col != x.Col && y.Area != x.Area && x.Num == 0)).ToList();
        }
        #endregion

        #region Expand
        /// <summary>
        /// 空きが1セルの個所があった場合、対象数値で埋める
        /// </summary>
        /// <param name="vCell"></param>
        /// <param name="vNum"></param>
        /// <returns></returns>
        public static bool SetNumWhenTheLastOne(this Cell vCell, int vNum, List<Cell> vCells)
        {
            if (vCells.Count(x => x.Row == vCell.Row) == 1 || vCells.Count(x => x.Col == vCell.Col) == 1 || vCells.Count(x => x.Area == vCell.Area) == 1)
            {
                vCell.SetNum(vNum);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 複製の作成
        /// </summary>
        /// <param name="vTarget"></param>
        /// <returns></returns>
        public static object DeepClone(this object vTarget)
        {
            BinaryFormatter wBinaryFormatter = new BinaryFormatter();
            MemoryStream wMemoryStream = new MemoryStream();

            try
            {
                wBinaryFormatter.Serialize(wMemoryStream, vTarget);
                wMemoryStream.Position = 0;
                return wBinaryFormatter.Deserialize(wMemoryStream);
            }
            finally
            {
                wMemoryStream.Close();
            }
        }

        /// <summary>
        /// セルに当てはまらない数の取得
        /// </summary>
        /// <param name="vCell"></param>
        /// <returns></returns>
        public static List<int> RemainNum(this Cell vCell)
        {
            var wRemains = Remains.Select(x => x.Num).ToList();
            return wRemains.Except(Cells.Where(x => x.Row == vCell.Row).Select(x => x.Num)).ToList()
                .Concat(wRemains.Except(Cells.Where(x => x.Col == vCell.Col).Select(x => x.Num)).ToList())
                .Concat(wRemains.Except(Cells.Where(x => x.Area == vCell.Area).Select(x => x.Num)).ToList())
                .Distinct().ToList();
        }

        /// <summary>
        /// 紐づく行、列、表に存在する値
        /// </summary>
        /// <param name="vCell"></param>
        /// <returns></returns>
        public static List<int> Exists(this Cell vCell)
        {
            return Cells.Where(x => x.Row == vCell.Row || x.Col == vCell.Col || x.Area == vCell.Area).Select(x => x.Num).Distinct().ToList();
        }
        #endregion

        public static void ShowMap() => MessageBox.Show(string.Join("\r\n", Enumerable.Range(0, 9).ToList().Select(i => string.Join(",", Cells.Where(x => x.Row == i).OrderBy(x => x.Col).Select(x => x.Num)))));
        public static bool CheckAns()
        {
            Func<IEnumerable<IGrouping<int, Cell>>, bool> wChecker = vList => vList.All(x => x.ToList().Select(y => y.Num).Distinct().Count() == 9);
            if (!wChecker(Cells.GroupBy(x => x.Row))) return false;
            if (!wChecker(Cells.GroupBy(x => x.Col))) return false;
            if (!wChecker(Cells.GroupBy(x => x.Area))) return false;
            return true;
        }
    }
}
