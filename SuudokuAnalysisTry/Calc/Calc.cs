using System.Linq;

namespace SuudokuAnalysisTry.Calc
{
    class Calc
    {
        /// <summary>
        /// メイン処理
        /// </summary>
        /// <returns>True:正常値、False：不整値入力あり</returns>
        public bool Exe()
        {
            
            // 誤った値が入力されていた場合、Falseを返す
            if (Map.Reset) return false;

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
                foreach (var wNum in wCell.RemainNum())
                {
                    wCell.SetNumTemp(wNum);

                    // 不正値入力が無ければ、正常終了
                    if (Exe()) return true;
                    Map.Reset = false;

                    // 仮設定値の初期化
                    Map.ClearNumTemp();
                }
            }
            #endregion

            // 誤った入力が無く、全セルが埋まっていない場合に、再起実行
            if (!Map.Reset && Map.Targets.Count > 0) Exe();

            // 誤った値が入力されていた場合、Falseを返す
            return !Map.Reset;
        }
    }
}
