using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NoteProject.Entity;

namespace NoteProject.Context
{
    public interface IDatabaseContext
    {


        DbSet<User> Users { set; get; }
        DbSet<Note> Notes { set; get; }
        DbSet<Like> Likes { set; get; }
        DbSet<Profile> Profile { set; get; }
        DbSet<Comment> Comment { set; get; }
        DbSet<Tag> Tags { set; get; }



        int SaveChanges(bool acceptAllChangesOnSuccess);

        int SaveChanges();

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        EntityEntry<TEntity> Attach<TEntity>([NotNull] TEntity entity) where TEntity : class;

        EntityEntry Attach([NotNull] object entity);

        //
        // Summary:
        //     Gets an Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry for the given
        //     entity. The entry provides access to change tracking information and operations
        //     for the entity.
        //     This method may be called on an entity that is not tracked. You can then set
        //     the Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State property on
        //     the returned entry to have the context begin tracking the entity in the specified
        //     state.
        //
        // Parameters:
        //   entity:
        //     The entity to get the entry for.
        //
        // Returns:
        //     The entry for the given entity.
        EntityEntry Entry([NotNull] object entity);
        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;
    }


}