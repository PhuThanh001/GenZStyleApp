using BMOS.DAL.DAOs;
using GenZStyleApp.DAL.DAO;
using GenZStyleApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectParticipantManagement.DAL.Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private GenZStyleDbContext _dbContext;
        private AccountDAO _accountDAO;
        private RoleDAO _roleDAO;
        private UserDAO _userDAO;
        private TokenDAO _tokenDAO;
        private PostDAO _postDAO;
        private HashTagDAO _hashtagDAO;
        
        private LikeDAO _likeDAO;
        private CommentDAO _commentDAO;
        private NotificationDAO _notificationDAO;
        private UserRelationDAO  _userRelationDAO;
        private CategoryDAO _categoryDAO;
        private ReportDAO _reportDAO;
        private CollectionDAO _collectionDAO;
        private HashPostDAO _hashPostDAO;
        private MessageDAO  _messageDAO;
        private PackageDAO _packageDAO;
        private InvoiceDAO _invoiceDAO;
        private PackageRegistrationDAO _packageRegistrationDAO;
        

        public UnitOfWork()
        {
            if (this._dbContext == null)
            {
                this._dbContext = DbFactory.Instance.InitDbContext();
            }
        }
        public RoleDAO RoleDAO
        {
            get
            {
                if (_roleDAO == null)
                {
                    _roleDAO = new RoleDAO(_dbContext);
                }
                return _roleDAO;
            }
        }
        public MessageDAO MessageDAO
        {
            get
            {
                if (_messageDAO == null)
                {
                    _messageDAO = new MessageDAO(_dbContext);
                }
                return _messageDAO;
            }
        }
        public HashPostDAO HashPostDAO
        {
            get
            {
                if (_hashPostDAO == null)
                {
                    _hashPostDAO = new HashPostDAO(_dbContext);
                }
                return _hashPostDAO;
            }
        }
        public UserRelationDAO userRelationDAO
        {
            get
            {
                if (_userRelationDAO == null)
                {
                    _userRelationDAO = new UserRelationDAO(_dbContext);
                }
                return _userRelationDAO;
            }
        }
        public NotificationDAO NotificationDAO
        {
            get
            {
                if (_notificationDAO == null)
                {
                    _notificationDAO = new NotificationDAO(_dbContext);
                }
                return _notificationDAO;
            }
        }
        public LikeDAO LikeDAO
        {
            get
            {
                if (_likeDAO == null)
                {
                    _likeDAO = new LikeDAO(_dbContext);
                }
                return _likeDAO;
            }
        }
        public PostDAO PostDAO
        {
            get
            {
                if (_postDAO == null)
                {
                    _postDAO = new PostDAO(_dbContext);
                }
                return _postDAO;
            }
        }
        public CommentDAO CommentDAO
        {
            get
            {
                if (_commentDAO == null)
                {
                    _commentDAO = new CommentDAO(_dbContext);
                }
                return _commentDAO;
            }
        }
        public TokenDAO TokenDAO
        {
            get
            {
                if (_tokenDAO == null)
                {
                    _tokenDAO = new TokenDAO(_dbContext);
                }
                return _tokenDAO;
            }
        }

        
        
        public AccountDAO AccountDAO
        {
            get
            {
                if (this._accountDAO == null)
                {
                    this._accountDAO = new AccountDAO(this._dbContext);
                }
                return this._accountDAO;
            }
        }
        public UserDAO UserDAO
        {
            get
            {
                if (this._userDAO == null)
                {
                    this._userDAO = new UserDAO(this._dbContext);
                }
                return this._userDAO;
            }
        }
      

        public HashTagDAO HashTagDAO
        {
            get
            {
                if (this._hashtagDAO == null)
                {
                    this._hashtagDAO = new HashTagDAO(this._dbContext);
                }
                return this._hashtagDAO;
            }
        }

        

        public CategoryDAO CategoryDAO
        {
            get
            {
                if (this._categoryDAO == null)
                {
                    this._categoryDAO = new CategoryDAO(this._dbContext);
                }
                return this._categoryDAO;
            }
        }

        public ReportDAO ReportDAO
        {
            get
            {
                if (this._reportDAO == null)
                {
                    this._reportDAO = new ReportDAO(this._dbContext);
                }
                return this._reportDAO;
            }
        }

        
        public CollectionDAO CollectionDAO
        {
            get
            {
                if (this._collectionDAO == null)
                {
                    this._collectionDAO = new CollectionDAO(this._dbContext);
                }
                return this._collectionDAO;
            }
        }
        public PackageDAO packageDAO
        {
            get
            {
                if (_packageDAO == null)
                {
                    _packageDAO = new PackageDAO(_dbContext);
                }
                return _packageDAO;
            }
        }
        public InvoiceDAO InvoiceDAO
        {
            get
            {
                if (_invoiceDAO == null)
                {
                    _invoiceDAO = new InvoiceDAO(_dbContext);
                }
                return _invoiceDAO;
            }
        }
        public PackageRegistrationDAO PackageRegistrationDAO
        {
            get
            {
                if (_packageRegistrationDAO == null)
                {
                    _packageRegistrationDAO = new PackageRegistrationDAO(_dbContext);
                }
                return _packageRegistrationDAO;
            }
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
