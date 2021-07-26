﻿using System.Collections.Generic;
    using UnityEngine;
public static class Preloading
    {
        private static List<IPreloadingAComponent> _preloadingAComponents = new List<IPreloadingAComponent>();
        private static Load _load;
        public static void Subscribe(IPreloadingAComponent preloadingAComponent)
        {
            _preloadingAComponents.Add(preloadingAComponent);
        }

        public static void Unsubscribe(IPreloadingAComponent preloadingAComponent)
        {
            _preloadingAComponents.Remove(preloadingAComponent);
        }

        public static void CheckPreloading()
        {
            foreach (var component in _preloadingAComponents)
            {
                if (component.PreloadingCompleted == false) return;
            }

            _load.PreloadingCompleted();
        }

        public static void Initialize(Load load)
        {
            _load = load;
        }
    }