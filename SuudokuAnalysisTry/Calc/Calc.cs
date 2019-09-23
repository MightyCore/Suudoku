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
                    if(wCell.TempCnt < wCell.RemainNum().Count)
                    {
                        // 仮で番号を記入
                        var wNum = wCell.RemainNum()[wCell.TempCnt];
                        wCell.SetNumTemp(wNum);
                    }
                    else
                    {
                        // 仮番号の初期化
                        wCell.TempCnt = 0;
                        Map.ClearNumTemp();
                    }
                    continue;
                }
                #endregion

                // 全ての値の記入が完了したら終了
                if (Map.Targets.Count == 0) break;

                // 仮番号の初期化
                Map.ClearNumTemp();
            }
        }
    }
}
