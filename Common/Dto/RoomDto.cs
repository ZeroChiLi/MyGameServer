using System;
using System.Collections.Generic;

namespace Common.Dto
{
    //房间数据传输对象，包含一条用户数据对象。
    [Serializable]
    public class RoomDto
    {
        public List<AccountDto> accountList;

        public RoomDto()
        {
            accountList = new List<AccountDto>();
        }
    }
}
