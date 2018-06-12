using System;

namespace Persistence.Entities
{
    public class LifeLike
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ForSurvival { get; set; }

        public string ForBirth { get; set; }

        public string Character { get; set; }
    }
}