/*
 * ============================================================================
 * Mozilla Public License 2.0 (MPL-2.0)
 * Autor: FISCAL API S. DE R.L. DE C.V. - https://fiscalapi.com
 * ============================================================================
 *
 * Este código está sujeto a los términos de la Mozilla Public License v2.0.
 * Licencia completa: https://mozilla.org/MPL/2.0
 *
 * AVISO: Este software se proporciona "tal como está" sin garantías de ningún
 * tipo. Al usar, modificar o distribuir este código debe mantener esta
 * atribución y las referencias al autor.
 *
 * ============================================================================
 */

namespace Fiscalapi.XmlDownloader.Common.Attributes;

/// <summary>
/// Declara el namespace URI del complemento SAT que materializa la propiedad decorada.
/// El match se hace por namespace (la única fuente de verdad del XML); el prefijo del
/// elemento es decorativo y lo elige el emisor, por lo que no debe usarse para matchear.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ComplementAttribute(string namespaceUri) : Attribute
{
    public string NamespaceUri { get; } = namespaceUri;
}