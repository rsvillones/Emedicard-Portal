﻿using Corelib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class ActionMemoViewModel
    {
        public ActionMemoViewModel()
        {
            Documents = new Collection<Document>();
        }

        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string MemberReply { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}