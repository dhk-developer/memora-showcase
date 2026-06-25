#if UNITY_EDITOR
using System.Collections.Generic;
using RhythmShowcase.Data;
using RhythmShowcase.Validation;
using UnityEditor;
using UnityEngine;

namespace RhythmShowcase.Editor
{
    /// <summary>
    /// Thin Unity Editor adapter around the pure validation service.
    /// </summary>
    public static class ShowcaseChartValidationMenu
    {
        [MenuItem("Window/Rhythm Showcase/Validate Sample Chart")]
        private static void ValidateSampleChart()
        {
            TextAsset asset = Resources.Load<TextAsset>("sample_chart");
            if (asset == null)
            {
                Debug.LogError("Could not find Resources/sample_chart.json.");
                return;
            }

            ShowcaseChart chart = JsonUtility.FromJson<ShowcaseChart>(asset.text);
            IReadOnlyList<ValidationIssue> issues = ChartValidator.Validate(chart);
            int errors = 0;
            int warnings = 0;

            foreach (ValidationIssue issue in issues)
            {
                if (issue.Severity == ValidationSeverity.Error)
                {
                    errors++;
                    Debug.LogError(issue.ToString(), asset);
                }
                else
                {
                    warnings++;
                    Debug.LogWarning(issue.ToString(), asset);
                }
            }

            string summary = $"Validation complete. Errors: {errors}; warnings: {warnings}.";
            if (errors == 0 && warnings == 0)
            {
                Debug.Log(summary, asset);
            }

            EditorUtility.DisplayDialog("Rhythm Showcase", summary + " See the Console for details.", "OK");
        }
    }
}
#endif
