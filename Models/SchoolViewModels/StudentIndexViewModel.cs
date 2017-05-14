﻿using System.Collections.Generic;

namespace KuetOverflow.Models.SchoolViewModels
{
    public class StudentIndexViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Tweet> Tweets { get; set; }
    }
}
