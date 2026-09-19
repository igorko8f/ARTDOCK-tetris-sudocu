using System;
using CodeBase.Gameplay.Board;
using CodeBase.Gameplay.Cells;
using CodeBase.Gameplay.Figures;
using CodeBase.Gameplay.Figures.Tray;

namespace CodeBase.Infrastructure.ResourcesProvider
{
    public static class ResourceNames
    {
        //Declare resources [type], [location in resources folder]
        private static ResourceName[] resources =
        {
            new (typeof(GameBoardConfiguration), "Configuration"),
            new (typeof(FigureConfiguration), "Configuration/Figures"),
            new (typeof(BoardCell), "Prefabs"),
            new (typeof(GameBoardView), "Prefabs"),
            new (typeof(FigureTray), "Prefabs"),
            new (typeof(Figure), "Prefabs"),
        };

        public static string GetLocation<TResource>() where TResource : IResource
        {
            var location = string.Empty;
            foreach (var resource in resources)
            {
                if (resource.Type == typeof(TResource))
                    location = resource.Location;
            }

            if (string.IsNullOrEmpty(location))
                throw new NullReferenceException($"The is no path for resource with type {typeof(TResource)}.");

            return location;
        }
    }
}
