using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ServerShared.Helpers;

/// <summary>
/// Helps creating certificates.
/// </summary>
public static class CertificateHelper
{
    /// <summary>
    /// Default password for 
    /// </summary>
    public const string DefaultCertPassword = "Shared";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="algorithm"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static X509Certificate2 Create(string name, AsymmetricAlgorithm algorithm, HashAlgorithmName hashAlgorithm, string password = DefaultCertPassword)
    {
        X500DistinguishedNameBuilder nameBuilder = new();
        nameBuilder.AddCommonName(name);
        return Create(name, algorithm, hashAlgorithm, nameBuilder, password);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="algorithm"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="nameBuilder"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static X509Certificate2 Create(string name, AsymmetricAlgorithm algorithm, HashAlgorithmName hashAlgorithm, X500DistinguishedNameBuilder nameBuilder, string password = DefaultCertPassword)
    {
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(nameBuilder);

        Span<byte> serialNumber = stackalloc byte[8];
        RandomNumberGenerator.Fill(serialNumber);
        var key = PublicKey.CreateFromSubjectPublicKeyInfo(algorithm.ExportSubjectPublicKeyInfo(), out _);
        CertificateRequest certificate = new(nameBuilder.Build(), key, hashAlgorithm);
        certificate.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, false));
        certificate.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(algorithm.ExportSubjectPublicKeyInfo(), false));
        X509SignatureGenerator? generator = null;
        if (algorithm is ECDsa ecdsa)
            generator = X509SignatureGenerator.CreateForECDsa(ecdsa);
        if (algorithm is RSA rsa)
            generator = X509SignatureGenerator.CreateForRSA(rsa, RSASignaturePadding.Pkcs1);
        Debug.Assert(generator != null);
        var validFrom = DateTimeOffset.Now;
        var validTo = validFrom.AddYears(10);
        var cert = certificate.Create(nameBuilder.Build(), generator, validFrom, validTo, serialNumber);
        string outPath = Path.Combine(Directory.GetCurrentDirectory(), "Cert");
        File.WriteAllText(Path.Combine(outPath, $"{name}.req.pem"), certificate.CreateSigningRequestPem());
        File.WriteAllBytes(Path.Combine(outPath, $"{name}.pfx"), cert.Export(X509ContentType.Pfx, password));
        File.WriteAllText(Path.Combine(outPath, $"{name}_private.key"), algorithm.ExportPkcs8PrivateKeyPem());
        File.WriteAllText(Path.Combine(outPath, $"{name}_public.key"), algorithm.ExportSubjectPublicKeyInfoPem());
        
        return cert;
    }
}
