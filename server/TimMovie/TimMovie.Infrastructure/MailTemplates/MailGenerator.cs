namespace TimMovie.Infrastructure.MailTemplates;

public class MailGenerator
{
    public static string GenerateMail(string header, string mainContent, string picture)
    {
        return "<div>\n" + "    <div id=\"style_16485872332082344426_BODY\"><div class=\"cl_389921\">\n" +
               "        <style>.cl_389921 body, .cl_389921 table, .cl_389921 td, .cl_389921 a{\n" +
               "            font-family:\"\\49\\6E\\74\\65\\72\", \"\\48\\65\\6C\\76\\65\\74\\69\\63\\61\", \"\\41\\72\\69\\61\\6C\", sans-serif;\n" +
               "        }.cl_389921 table, .cl_389921 td{\n" + "         }.cl_389921 img{\n" +
               "          }.cl_389921 img{\n" + "               border-top-color:currentColor;\n" +
               "               border-top-style:none;\n" + "               border-top-width:0px;\n" +
               "               border-right-color:currentColor;\n" + "               border-right-style:none;\n" +
               "               border-right-width:0px;\n" + "               border-bottom-color:currentColor;\n" +
               "               border-bottom-style:none;\n" + "               border-bottom-width:0px;\n" +
               "               border-left-color:currentColor;\n" + "               border-left-style:none;\n" +
               "               border-left-width:0px;\n" + "               height:auto;\n" +
               "               line-height:100%;\n" + "               outline-color:invert;\n" +
               "               outline-style:none;\n" + "               outline-width:medium;\n" +
               "               text-decoration:none;\n" + "           }.cl_389921 ul, .cl_389921 ol{\n" +
               "                margin-top:0px !important;\n" + "                margin-right:0px !important;\n" +
               "                margin-bottom:0px !important;\n" + "                margin-left:0px !important;\n" +
               "                margin-left:25px !important;\n" + "                padding-top:0px !important;\n" +
               "                padding-right:0px !important;\n" + "                padding-bottom:0px !important;\n" +
               "                padding-left:0px !important;\n" +
               "            }.cl_389921 ul li, .cl_389921 ol li, .cl_389921 p{\n" +
               "                 margin-top:16px;\n" + "                 margin-bottom:16px;\n" +
               "             }.cl_389921 table{\n" + "                  border-collapse:collapse !important;\n" +
               "              }.cl_389921 body{\n" + "                   height:100% !important;\n" +
               "                   margin-top:0px !important;\n" + "                   margin-right:0px !important;\n" +
               "                   margin-bottom:0px !important;\n" +
               "                   margin-left:0px !important;\n" + "                   padding-top:0px !important;\n" +
               "                   padding-right:0px !important;\n" +
               "                   padding-bottom:0px !important;\n" +
               "                   padding-left:0px !important;\n" + "                   width:100% !important;\n" +
               "               }.cl_389921 h2{\n" + "                    font-weight:400;\n" +
               "                }.cl_389921 #MessageViewBody_mr_css_attr a{\n" +
               "                     color:inherit;\n" + "                     text-decoration:none;\n" +
               "                     font-size:inherit;\n" + "                     font-family:inherit;\n" +
               "                     font-weight:inherit;\n" + "                     line-height:inherit;\n" +
               "                 }@media screen and (max-width: 580px){\n" +
               "            .cl_389921 .mobile-padding_mr_css_attr{\n" +
               "                padding-left:20px !important;\n" + "                padding-right:20px !important;\n" +
               "            }.cl_389921 .content-mobile-padding_mr_css_attr, .cl_389921 .mobile-hero-padding_mr_css_attr{\n" +
               "                 padding-left:40px !important;\n" +
               "                 padding-right:40px !important;\n" +
               "             }.cl_389921 .email-title_mr_css_attr{\n" +
               "                  font-size:33px !important;\n" + "                  line-height:42px !important;\n" +
               "              }.cl_389921 p, .cl_389921 li, .cl_389921 .link_mr_css_attr, .cl_389921 .td-content_mr_css_attr{\n" +
               "                   font-size:22px !important;\n" + "                   line-height:39px !important;\n" +
               "               }.cl_389921 .button_mr_css_attr a{\n" +
               "                    max-width:100% !important;\n" + "                    width:100% !important;\n" +
               "                    font-size:28px !important;\n" +
               "                    line-height:49px !important;\n" +
               "                }.cl_389921 .h3-text_mr_css_attr, .cl_389921 .address-content_mr_css_attr, .cl_389921 .address-content_mr_css_attr span, .cl_389921 .sent-by-content_mr_css_attr, .cl_389921 .sent-by-content_mr_css_attr span{\n" +
               "                     font-size:15px !important;\n" +
               "                     line-height:26px !important;\n" +
               "                 }.cl_389921 .unsubscribe-content_mr_css_attr, .cl_389921 .unsubscribe-content_mr_css_attr a{\n" +
               "                      font-size:14px !important;\n" +
               "                      line-height:24px !important;\n" + "                  }\n" +
               "        }</style>\n" + "\n" +
               "        <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n" +
               "            <tbody><tr>\n" +
               "                <td class=\"mobile-padding_mr_css_attr\" align=\"center\" valign=\"top\" width=\"100%\" bgcolor=\"#0F0D1D\"   style=\"padding: 40px 0;background-color: #0F0D1D;\">\n" +
               "\n" +
               "                    <table role=\"presentation\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:580px;min-width:580px;margin: 0 auto;\">\n" +
               "                        <tbody><tr>\n" +
               "                            <td align=\"center\" valign=\"top\" bgcolor=\"#1F1C2E;\" style=\"font-family: 'Inter', 'Helvetica', 'Arial', sans-serif;border-radius: 25px;padding-top:40px;box-shadow: 0px 2px 20px 0px rgb(31,10,56);background-color: #1F1C2E;\">\n" +
               "\n" + "\n" +
               "                                <table role=\"presentation\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: center;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" + "                                    <tr>\n" +
               "                                        <td class=\"content-mobile-padding_mr_css_attr\" align=\"center\" valign=\"top\" style=\"padding: 0 40px;\">\n" +
               "                                                                                        <a href=\"\" target=\"_blank\" align=\"center\" style=\"cursor: pointer;margin: 0;font-weight: normal;line-height: 1;margin: 0;padding: 0;text-align: left;text-decoration: none;text-underline: none\" rel=\" noopener noreferrer\">"
            +
               "                                                <img src=\"https://i.ibb.co/khHMtV3/TMOVIE-free-file-3.png\" width=\"150\" height=\"74\" border=\"0\">\n" +
               "                                            </a>\n" +
               "                                        </td>\n" + "                                    </tr>\n" +
               "                                    </tbody>\n" + "                                </table>\n" + "\n" +
               "\n" + "\n" +
               "                                <table role=\"presentation\" class=\"spacer_mr_css_attr\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" +
               "                                    <tr style=\"padding: 0;text-align: left;vertical-align: top\">\n" +
               "                                        <td align=\"center\" valign=\"top\" height=\"30px\" style=\"-moz-hyphens: auto;-webkit-hyphens: auto;margin: 0;border-collapse: collapse !important;color: #001d2f;font-size: 30px;font-weight: normal;hyphens: auto;line-height: 30px;margin: 0;padding: 0;text-align: left;vertical-align: top;word-wrap: break-word\" class=\"MsoNormal_mr_css_attr\"> &nbsp;</td>\n" +
               "                                    </tr>\n" + "                                    </tbody>\n" +
               "                                </table>\n" + "\n" + "\n" + "\n" +
               "                                <table role=\"presentation\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" +
               "                                    <tr style=\"font-family:Arial,sans-serif;font-size:18px;line-height:21px;color:#ffffff;font-weight:bold;display: flex;justify-content: center\">\n" +
               "                                        <td class=\"content-mobile-padding_mr_css_attr\" align=\"left\" valign=\"top\" style=\"padding: 0 40px;\">\n" +
               "                                            <h1 class=\"email-title_mr_css_attr\" style=\"margin:0;padding:0;font-size: 32px;font-weight: 700;font-family: 'Inter', 'Helvetica', 'Arial', sans-serif;letter-spacing: 0px;line-height: 32px;\">\n" +
               "                                                Плотный салам всем нашим\n" +
               "                                            </h1>\n" +
               "                                        </td>\n" + "                                    </tr>\n" +
               "                                    </tbody>\n" + "                                </table>\n" + "\n" +
               "                                <table role=\"presentation\" class=\"spacer_mr_css_attr\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" +
               "                                    <tr style=\"padding: 0;text-align: left;vertical-align: top\">\n" +
               "                                        <td align=\"center\" valign=\"top\" height=\"35px\" style=\"-moz-hyphens: auto;-webkit-hyphens: auto;margin: 0;border-collapse: collapse !important;color: #001d2f;font-size: 35px;font-weight: normal;hyphens: auto;line-height: 35px;margin: 0;padding: 0;text-align: left;vertical-align: top;word-wrap: break-word\" class=\"MsoNormal_mr_css_attr\"> &nbsp;</td>\n" +
               "                                    </tr>\n" + "                                    </tbody>\n" +
               "                                </table>\n" + "\n" + "\n" +
               "                                <table role=\"presentation\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: center;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" + "                                    <tr>\n" +
               "                                        <td class=\"mobile-hero-padding_mr_css_attr\" align=\"center\" valign=\"top\" style=\"padding: 0 40px;\">\n" +
               $"                                            <img src=\"{picture}\" alt=\"Three houseplants hide digits in an IP address.\" width=\"500\" height=\"300\" border=\"0\" style=\"display: block;margin: 0 auto;border-radius: 20px;\">\n" +
               "                                        </td>\n" + "                                    </tr>\n" +
               "                                    </tbody>\n" + "                                </table>\n" + "\n" +
               "\n" + "\n" + "\n" +
               "                                <table role=\"presentation\" class=\"spacer_mr_css_attr\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" +
               "                                    <tr style=\"padding: 0;text-align: left;vertical-align: top\">\n" +
               "                                        <td align=\"center\" valign=\"top\" height=\"50px\" style=\"-moz-hyphens: auto;-webkit-hyphens: auto;margin: 0;border-collapse: collapse !important;color: #001d2f;font-size: 20px;font-weight: normal;hyphens: auto;line-height: 20px;margin: 0;padding: 0;text-align: left;vertical-align: top;word-wrap: break-word\" class=\"MsoNormal_mr_css_attr\"> &nbsp;</td>\n" +
               "                                    </tr>\n" + "                                    </tbody>\n" +
               "                                </table>\n" + "\n" + "\n" +
               "                                <table role=\"presentation\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" + "                                    <tr>\n" +
               "                                        <td class=\"content-mobile-padding_mr_css_attr td-content_mr_css_attr\" align=\"left\" valign=\"top\" style=\"padding: 0 40px;font-family:Arial,sans-serif;font-size:22px;color:#ffffff;font-weight:bold;line-height: 28px;\">\n" +
               "\n" + "\n" + $"                                            {header}\n" + "\n" + "\n" +
               "                                            <table role=\"presentation\" class=\"spacer_mr_css_attr\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                                <tbody>\n" +
               "                                                <tr style=\"padding: 0;text-align: left;vertical-align: top\">\n" +
               "                                                    <td align=\"center\" valign=\"top\" height=\"20px\" style=\"-moz-hyphens: auto;-webkit-hyphens: auto;margin: 0;border-collapse: collapse !important;color: #001d2f;font-size: 20px;font-weight: normal;hyphens: auto;line-height: 20px;margin: 0;padding: 0;text-align: left;vertical-align: top;word-wrap: break-word\" class=\"MsoNormal_mr_css_attr\"> &nbsp;</td>\n" +
               "                                                </tr>\n" +
               "                                                </tbody>\n" +
               "                                            </table>\n" +
             $"                                               {mainContent}\n" +
               "                                            <table role=\"presentation\" class=\"spacer_mr_css_attr\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                                <tbody>\n" +
               "                                                <tr style=\"padding: 0;text-align: left;vertical-align: top\">\n" +
               "                                                    <td align=\"center\" valign=\"top\" height=\"35px\" style=\"-moz-hyphens: auto;-webkit-hyphens: auto;margin: 0;border-collapse: collapse !important;color: #001d2f;font-size: 20px;font-weight: normal;hyphens: auto;line-height: 20px;margin: 0;padding: 0;text-align: left;vertical-align: top;word-wrap: break-word\" class=\"MsoNormal_mr_css_attr\"> &nbsp;</td>\n" +
               "                                                </tr>\n" +
               "                                                </tbody>\n" +
               "                                            </table>\n" +
               "                                        </td>\n" + "                                    </tr>\n" +
               "                                    </tbody>\n" + "                                </table>\n" + "\n" +
               "                                <table role=\"presentation\" style=\"border: none;border-top:1px solid #f0eacf;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" + "                                    <tr>\n" +
               "                                        <td class=\"content-mobile-padding_mr_css_attr\" align=\"left\" valign=\"top\" style=\"padding: 15px 40px;\">\n" +
               "                                            <table role=\"presentation\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                                <tbody>\n" +
               "                                                <tr>\n" +
               "                                                    <td class=\"sent-by-content_mr_css_attr\" align=\"left\" valign=\"middle\" height=\"24\" width=\"20\" style=\"width:24px;line-height: 20px;\">\n" +
               "                                                        <img src=\"https://i.ibb.co/K7M35KV/logo-free-file.png\" width=\"24\" height=\"20\" border=\"0\" style=\"display: block;vertical-align: top;\">\n" +
               "                                                    </td>\n" +
               "                                                    <td class=\"sent-by-content_mr_css_attr\" align=\"left\" valign=\"middle\" height=\"20\" width=\"10\" style=\"width:10px;line-height: 20px;\">\n" +
               "                                                        &nbsp;\n" +
               "                                                    </td>\n" +
               "                                                    <td class=\"sent-by-content_mr_css_attr\" align=\"left\" valign=\"middle\" height=\"20\" width=\"470\" style=\"color: gray;width:470px;font-size: 10px;font-weight: 400;font-family: 'Inter', 'Helvetica', 'Arial', sans-serif;letter-spacing: 0px;line-height: 20px;\">\n" +
               "                                                        <span>От души TMovie</span>\n" +
               "                                                    </td>\n" +
               "                                                </tr>\n" +
               "                                                </tbody>\n" +
               "                                            </table>\n" +
               "                                        </td>\n" + "                                    </tr>\n" +
               "                                    </tbody>\n" + "                                </table>\n" + "\n" +
               "                                <table role=\"presentation\" style=\"border: none;border-top:1px solid #f0eacf;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" + "                                    <tr>\n" +
               "                                        <td class=\"content-mobile-padding_mr_css_attr\" align=\"left\" valign=\"top\" style=\"padding: 20px 40px;\">\n" +
               "                                            <table role=\"presentation\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                                <tbody>\n" +
               "                                                <tr>\n" +
               "                                                    <td class=\"unsubscribe-content_mr_css_attr\" align=\"left\" valign=\"top\" height=\"9\" style=\"color: #0f5086;text-align:left;font-size: 9px;font-weight: 700;font-family: 'Inter', 'Helvetica', 'Arial', sans-serif;letter-spacing: 1.2px;line-height: 9px;text-transform: uppercase;text-decoration:none;\">\n" +
               "                                                        <a class=\"link_mr_css_attr\" style=\"color: #0f5086;text-decoration:none;\" href=\"https://www.youtube.com/watch?v=cws6MjTV7L0\" target=\"_blank\" rel=\" noopener noreferrer\">Поддержка</a>\n" +
               "                                                    </td>\n" +
               "                                                    <td class=\"unsubscribe-content_mr_css_attr\" align=\"center\" valign=\"top\" height=\"9\" style=\"color: #0f5086;text-align:center;font-size: 9px;font-weight: 700;font-family: 'Inter', 'Helvetica', 'Arial', sans-serif;letter-spacing: 1.2px;line-height: 9px;text-transform: uppercase;text-decoration:none;\">\n" +
               "                                                        <a class=\"link_mr_css_attr\" style=\"color: #0f5086;text-decoration:none;\" href=\"https://censor.net\" target=\"_blank\" rel=\" noopener noreferrer\">Политика конфиденциальности</a>\n" +
               "                                                    </td>\n" +
               "                                                    <td class=\"unsubscribe-content_mr_css_attr\" align=\"right\" valign=\"top\" height=\"9\" style=\"color: #0f5086;text-align:right;font-size: 9px;font-weight: 700;font-family: 'Inter', 'Helvetica', 'Arial', sans-serif;letter-spacing: 1.2px;line-height: 9px;text-transform: uppercase;text-decoration:none;\">\n" +
               "                                                        <a class=\"link_mr_css_attr\" style=\"color: #0f5086;text-decoration:none;\" href=\"https://natribu.org/ru/\" target=\"_blank\" rel=\" noopener noreferrer\">Прикол</a>\n" +
               "                                                    </td>\n" +
               "                                                </tr>\n" +
               "                                                </tbody>\n" +
               "                                            </table>\n" +
               "                                        </td>\n" + "                                    </tr>\n" +
               "                                    </tbody>\n" + "                                </table>\n" + "\n" +
               "                                <table role=\"presentation\" style=\"border: none;border-top:1px solid #f0eacf;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                    <tbody>\n" + "                                    <tr>\n" +
               "                                        <td class=\"content-mobile-padding_mr_css_attr\" align=\"left\" valign=\"top\" style=\"padding: 20px 40px;\">\n" +
               "                                            <table role=\"presentation\" style=\"border: none;border-collapse: collapse;border-spacing: 0;padding: 0;text-align: left;vertical-align: top;width: 100%\">\n" +
               "                                                <tbody>\n" +
               "                                                <tr>\n" +
               "                                                    <td class=\"address-content_mr_css_attr\" align=\"left\" valign=\"middle\" height=\"10\" width=\"470\" style=\"color: gray;width:470px;font-size: 10px;font-weight: 400;font-family: 'Inter', 'Helvetica', 'Arial', sans-serif;letter-spacing: 0px;line-height: 10px;\">\n" +
               "                                                        <span>Татарстан емае</span>\n" +
               "                                                    </td>\n" +
               "                                                </tr>\n" +
               "                                                </tbody>\n" +
               "                                            </table>\n" +
               "                                        </td>\n" + "                                    </tr>\n" +
               "                                    </tbody>\n" + "                                </table>\n" + "\n" +
               "\n" + "                            </td>\n" + "                        </tr>\n" +
               "                        </tbody></table>\n" + "\n" + "                </td>\n" + "            </tr>\n" +
               "            </tbody></table>\n" + "\n" + "\n" +
               "        <style>.MsoNormal_mr_css_attr{margin:0;}</style></div></div>\n" + "</div>";
    }
}