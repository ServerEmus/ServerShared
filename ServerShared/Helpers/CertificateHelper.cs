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
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="algorithm"></param>
    /// <param name="hashAlgorithm"></param>
    /// <returns></returns>
    public static X509Certificate2 Create(string name, AsymmetricAlgorithm algorithm, HashAlgorithmName hashAlgorithm)
    {
        X500DistinguishedNameBuilder nameBuilder = new();
        nameBuilder.AddCommonName(name);
        return Create(name, algorithm, hashAlgorithm, nameBuilder);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="algorithm"></param>
    /// <param name="hashAlgorithm"></param>
    /// <param name="nameBuilder"></param>
    /// <returns></returns>
    public static X509Certificate2 Create(string name, AsymmetricAlgorithm algorithm, HashAlgorithmName hashAlgorithm, X500DistinguishedNameBuilder nameBuilder)
    {
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
        File.WriteAllBytes(Path.Combine(outPath, $"{name}.pfx"), cert.Export(X509ContentType.Pfx));
        File.WriteAllText(Path.Combine(outPath, $"{name}_private.key"), algorithm.ExportPkcs8PrivateKeyPem());
        File.WriteAllText(Path.Combine(outPath, $"{name}_public.key"), algorithm.ExportSubjectPublicKeyInfoPem());
        return cert;
    }
}
