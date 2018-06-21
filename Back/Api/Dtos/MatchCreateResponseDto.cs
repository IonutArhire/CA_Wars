using System;

namespace Api.Dtos
{
    public class MatchCreateResponseDto
    {
        public MatchCreateResponseDto(Guid id) 
        {
            this.Id = id;
        }
        
        public Guid Id { get; set; }
    }
}