using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class CommentService
    {
        private UnitOfWork _unitOfWork;
        public CommentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Comment comment)
        {
            await _unitOfWork.Comments.CreateAsync(comment);
            await _unitOfWork.Save();
        }

    }
}
