namespace Biograf.Domain.Interfaces;
 
/// <summary>
/// Data access for screenings.
/// </summary>
public interface IScreening
{
     Task<List<Screening>> GetAllAsync();
     Task<Screening?> GetByIdAsync(int id);
     Task<Screening> AddAsync(Screening screening);
     Task<bool> UpdateAsync(Screening screening);
     Task<bool> DeleteAsync(int id);
     Task<Screening?> GetWithHallAndSeatsAsync(int id);
}
