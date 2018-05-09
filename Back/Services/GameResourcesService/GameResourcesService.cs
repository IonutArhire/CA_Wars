using System;

namespace Services.GameResourcesService
{
    public static class GameResourcesService
    {
        private static int size = 30;
        public static Object getGameResources() {
            var playerResources = PlayerResourcesService.PlayerResourcesService.getPlayerResources();
            return new {size = size, playerResources = playerResources};
        }
    }
}