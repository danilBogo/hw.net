using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure.Database;

public class ApplicationContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<CommentReport> CommentReports { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Film> Films { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Producer> Producers { get; set; }
    public DbSet<Subscribe?> Subscribes { get; set; }
    public DbSet<UserFilmWatched> WatchedFilms { get; set; }
    public DbSet<UserSubscribe> UserSubscribes { get; set; }
    public DbSet<IdentityUserClaim<Guid>> Claims { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatTemplatedNotification> ChatTemplatedNotifications { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseOpenIddict();
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasMany(x => x.FilmsWatchLater).WithMany(x => x.UsersWatchLater);
        modelBuilder.Entity<User>().HasOne(x => x.WatchingFilm).WithMany(x => x.UsersWatching)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>().HasOne(x => x.Country).WithMany(x => x.Users).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Film>().HasOne(x => x.Country).WithMany(x => x.Films).OnDelete(DeleteBehavior.SetNull);
    }
}