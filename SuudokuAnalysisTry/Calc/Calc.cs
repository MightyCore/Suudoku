using System.Linq;

namespace SuudokuAnalysisTry.Calc
{
    class Calc
    {
        /// <summary>
        /// メイン処理
        /// </summary>
        public void Exe()
        {
            while (true)
            {
                #region FirstBlock
                while (true)
                {
                    // 行、列、エリアの残1セルに番号記入
                    if (Map.Targets.Any(x =>
                    {
                        var wCells = Map.GetZeroCellsWithoutNum(x);
                        return wCells.Count(y => y.SetNumWhenTheLastOne(x, wCells)) > 0;
                        // 記入セルがあれば再開
                    })) continue;
                    break;
                }
                #endregion

                #region SecondBlock
                // 0セルを、候補数値の少ない順に取得
                var wRemainCells = Map.Targets.SelectMany(x => Map.GetZeroCellsWithoutNum(x)).Distinct().OrderBy(x => x.RemainNum().Count()).ToList();
                if (wRemainCells.Count > 0)
                {
                    var wCell = wRemainCells.First();
                    var wNum = wCell.RemainNum()[wCell.TempCnt];
                    wCell.SetNumTemp(wNum);
                    continue;
                }
                #endregion

                if (Map.Targets.Count == 0) break;
                Map.ClearNumTemp();
            }
        }
    }
}
