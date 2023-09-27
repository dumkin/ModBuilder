// using System.Collections.Generic;
// using System.IO;
// using System.Threading.Tasks;
// using Curse.Integration.Models;
// using Curse.Integration.Models.Fingerprints;
//
// namespace Curse.Integration.C;
//
// public partial class ApiClient
// {
//     public async Task<GenericResponse<FingerprintsMatchesResult>> GetFingerprintByGameIdMatchesAsync(
//         uint gameId,
//         GetFingerprintMatchesRequestBody body
//     )
//     {
//         return await POST<GenericResponse<FingerprintsMatchesResult>>($"/v1/fingerprints/{gameId}", body);
//     }
//
//     public async Task<GenericResponse<FingerprintsMatchesResult>> GetFingerprintMatchesAsync(
//         GetFingerprintMatchesRequestBody body
//     )
//     {
//         return await POST<GenericResponse<FingerprintsMatchesResult>>("/v1/fingerprints", body);
//     }
//
//     public async Task<GenericResponse<FingerprintFuzzyMatchResult>> GetFingerprintsFuzzyMatchesByGameIdAsync(
//         uint gameId,
//         GetFuzzyMatchesRequestBody body
//     )
//     {
//         return await POST<GenericResponse<FingerprintFuzzyMatchResult>>($"/v1/fingerprints/fuzzy/{gameId}", body);
//     }
//
//     public async Task<GenericResponse<FingerprintFuzzyMatchResult>> GetFingerprintsFuzzyMatchesAsync(
//         GetFuzzyMatchesRequestBody body
//     )
//     {
//         return await POST<GenericResponse<FingerprintFuzzyMatchResult>>("/v1/fingerprints/fuzzy", body);
//     }
//
//     public async Task<GenericResponse<FingerprintsMatchesResult>> GetFingerprintMatchesForFileByGameIdAsync(
//         uint gameId,
//         string file
//     )
//     {
//         var fingerpruint = GetFingerprintFromFile(file);
//
//         return await POST<GenericResponse<FingerprintsMatchesResult>>($"/v1/fingerprints/{gameId}",
//             new GetFingerprintMatchesRequestBody
//             {
//                 Fingerprints = new List<long> { fingerpruint }
//             });
//     }
//
//     public async Task<GenericResponse<FingerprintsMatchesResult>> GetFingerprintMatchesForFileAsync(string file)
//     {
//         var fingerpruint = GetFingerprintFromFile(file);
//
//         return await POST<GenericResponse<FingerprintsMatchesResult>>("/v1/fingerprints",
//             new GetFingerprintMatchesRequestBody
//             {
//                 Fingerprints = new List<long> { fingerpruint }
//             });
//     }
//
//     public long GetFingerprintFromFile(string file)
//     {
//         return GetFingerprintFromBytes(File.ReadAllBytes(file));
//     }
//
//     public long GetFingerprintFromBytes(byte[] fileBytes)
//     {
//         return MurmurHash2.Hash(MurmurHash2.NormalizeByteArray(fileBytes));
//     }
// }

