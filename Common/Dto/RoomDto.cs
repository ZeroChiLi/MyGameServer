using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Dto
{
    public class RoomDto
    {
        public List<AccountDto> accountList;

        public RoomDto()
        {
            accountList = new List<AccountDto>();
        }
    }
}
