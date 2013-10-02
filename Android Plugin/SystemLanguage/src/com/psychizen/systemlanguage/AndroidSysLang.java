package com.psychizen.systemlanguage;

import java.util.Locale;

public class AndroidSysLang {
	public static String langCode;

	public static void GetSysLanguage()
	{
		langCode = Locale.getDefault().toString();
	}

}
