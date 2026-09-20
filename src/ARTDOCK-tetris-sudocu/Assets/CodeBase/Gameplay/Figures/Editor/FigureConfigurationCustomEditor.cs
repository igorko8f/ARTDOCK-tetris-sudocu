using CodeBase.Gameplay.Figures;
using UnityEditor;
using UnityEngine;
using CodeBase.Gameplay.Common.Extensions;

namespace CodeBase.Gameplay.Board.Figures.Editor
{
    [CustomEditor(typeof(FigureConfiguration))]
    public class FigureConfigurationCustomEditor : UnityEditor.Editor
    {
        private const float CellSize = 20f;

        public override void OnInspectorGUI()
        {
            var figure = (FigureConfiguration)target;

            EditorGUILayout.LabelField("Figure Form", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            DrawMatrix(figure);

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Clear")) 
                ClearMatrix(figure);

            EditorGUILayout.Space(5);
            DrawOccupiedCellsInfo(figure);
        }

        private void DrawMatrix(FigureConfiguration figure)
        {
            for (int y = 0; y < FigureConfiguration.Size; y++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int x = 0; x < FigureConfiguration.Size; x++)
                {
                    var currentValue = figure.Matrix[x, y];
                    var newValue = GUI.Toggle(
                        GUILayoutUtility.GetRect(
                            CellSize,
                            CellSize,
                            GUILayout.Width(CellSize),
                            GUILayout.Height(CellSize)
                        ),
                        currentValue,
                        ""
                    );

                    if (newValue == currentValue) 
                        continue;
                    
                    if (newValue && !CanActivateCell(figure, x, y))
                        continue;

                    if (!newValue && !CanDeactivateCell(figure, x, y))
                        continue;
                    
                    Undo.RecordObject(figure, "Change Figure Cell");

                    figure.Matrix[x, y] = newValue;
                    EditorUtility.SetDirty(figure);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void ClearMatrix(FigureConfiguration figure)
        {
            Undo.RecordObject(figure, "Clear Figure");

            for (int y = 0; y < FigureConfiguration.Size; y++)
            {
                for (int x = 0; x < FigureConfiguration.Size; x++) 
                    figure.Matrix[x, y] = false;
            }

            EditorUtility.SetDirty(figure);
        }

        private void DrawOccupiedCellsInfo(FigureConfiguration figure) => 
            EditorGUILayout.LabelField($"Occupied cells: {figure.Matrix.OccupiedCells()}");

        private bool CanActivateCell(FigureConfiguration figure, int x, int y) => 
            figure.Matrix.IsEmpty() || figure.Matrix.HasActiveNeighbour(x, y);

        private bool CanDeactivateCell(FigureConfiguration figure, int x, int y)
        {
            var matrix = figure.Matrix;
            var tmpValue = matrix[x, y];

            matrix[x, y] = false;
            var isConnected = matrix.IsConnected();
            
            matrix[x, y] = tmpValue;
            return isConnected;
        }
    }
}